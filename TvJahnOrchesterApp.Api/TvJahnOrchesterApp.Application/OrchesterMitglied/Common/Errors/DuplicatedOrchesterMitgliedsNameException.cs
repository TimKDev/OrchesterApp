using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.OrchesterMitglied.Common.Errors
{
    public class DuplicatedOrchesterMitgliedsNameException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public string Title => "Duplizierter Name";
        public string ErrorMessage { get; }

        public DuplicatedOrchesterMitgliedsNameException(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
