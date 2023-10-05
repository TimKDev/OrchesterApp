using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Models
{
    public record TerminRückmeldungsResponse(Guid TerminId, string Name, (string Vorname, string Nachname, string? VornameOther, string? NachnameOther, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied)[] TerminRückmeldungOrchestermitglied);
}
