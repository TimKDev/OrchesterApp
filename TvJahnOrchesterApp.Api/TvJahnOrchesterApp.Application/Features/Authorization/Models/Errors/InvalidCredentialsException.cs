using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors
{
    internal class InvalidCredentialsException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string Title => "Ungültige Login Daten";

        public string ErrorMessage => "Die eingegebenen Logindaten sind nicht valide.";
    }
}
