using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors
{
    internal class InvalidRegistrationKeyException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Title => "Ungültiger Registrierungschlüssel";
        public string ErrorMessage => "Der eingegebene Registrieungschlüssel ist ungültig, abgelaufen oder wurde bereits verwendet.";

        public InvalidRegistrationKeyException()
        {
        }
    }
}
