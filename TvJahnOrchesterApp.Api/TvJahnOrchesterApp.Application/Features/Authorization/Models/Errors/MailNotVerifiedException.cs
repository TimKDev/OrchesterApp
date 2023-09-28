using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors
{
    internal class MailNotVerifiedException : Exception, IServiceException
    {
        private readonly string userEmail;

        public MailNotVerifiedException(string userEmail)
        {
            this.userEmail = userEmail;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public string Title => "E-Mail nicht verifiziert.";

        public string ErrorMessage => $"Eine Verifizierungs Mail wurde an die Adresse {userEmail} versendet. Bitte klicken Sie auf den Verifizierungslink in der Mail.";
    }
}
