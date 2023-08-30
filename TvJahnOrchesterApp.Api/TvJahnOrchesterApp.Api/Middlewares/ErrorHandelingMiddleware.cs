using Microsoft.AspNetCore.Mvc;
using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Api.Middlewares
{
    public class ErrorHandelingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandelingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Diese Methode wird automatisch vor jedem Http Request ausgeführt, wenn diese Klasse der Pipeline als Middleware hinzugefügt wird.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Hier wird der Request des Users ausgeführt, d.h. wenn hier irgendwo ein Fehler bei der Bearbeitung des Http Requests auftritt, wird der Catch Block ausgeführt => Globales Error Handeling für ein Request.
                await _next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case IServiceException:
                        await HandleServiceException(context, (IServiceException)ex);
                        break;
                    default:
                        await HandleUnknownExceptionAsync(context, ex);
                        break;
                }
            }
        }

        private Task HandleServiceException(HttpContext context, IServiceException ex) 
        {
            context.Response.ContentType = "application/json";
            var problemDetails = new ProblemDetails
            {
                Title = ex.Title,
                Status = (int)ex.StatusCode,
                Detail = ex.ErrorMessage,
            };
            context.Response.StatusCode = (int)ex.StatusCode;
            return context.Response.WriteAsJsonAsync(problemDetails);
        }

        private Task HandleUnknownExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var problemDetails = new ProblemDetails
            {
                Title = "Unbekannter Fehler",
                Status = (int)code,
                Detail = "Ein unbekannter Fehler ist während der Server Abfrage aufgetreten.",
            };
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
