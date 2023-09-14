using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Common
{
    public record RückmeldungsResponse(Domain.OrchesterMitgliedAggregate.OrchesterMitglied Orchestermitglied, bool Zugesagt, string? Kommentar, TerminId TerminId);
}
