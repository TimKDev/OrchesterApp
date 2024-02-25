using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors
{
    internal class NewLoginNecessaryException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Title => "Neue Anmeldung erforderlich";
        public string ErrorMessage => "Der gespeicherte Anmeldungsschlüssel ist nicht mehr valide.  Bitte melden Sie sich erneut an.";
    }
}
