using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Common
{
    public record TerminResponse(Domain.TerminAggregate.Termin Termin, (string Vorname, string Nachname, string? VornameOther, string? NachnameOther, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied)[] TerminRückmeldungOrchestermitglied);
}
