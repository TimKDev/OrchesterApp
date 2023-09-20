using System.Net;
using TvJahnOrchesterApp.Application.Common.Errors;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Common.Errors
{
    internal class TerminIdNotFoundException : Exception, IServiceException
    {
        private readonly TerminId terminId;
        public TerminIdNotFoundException(TerminId terminId)
        {
            this.terminId = terminId;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string Title => "Termin nicht gefunden";

        public string ErrorMessage => $"Termin mit der Id {terminId.Value} wurde nicht gefunden. Bitte überprüfen Sie, dass die TerminId korrekt ist.";
    }
}
