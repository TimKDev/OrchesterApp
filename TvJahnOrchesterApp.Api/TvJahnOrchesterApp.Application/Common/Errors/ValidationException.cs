using System.Net;

namespace TvJahnOrchesterApp.Application.Common.Errors
{
    public class ServiceValidationException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Title => "Validierungsfehler";
        public string ErrorMessage { get; }

        public ServiceValidationException(string errorMessage) 
        {
            ErrorMessage = errorMessage;
        }
    }
}
