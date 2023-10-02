using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Dto
{
    public record TerminRückmeldungOrchestermitgliedDto(Guid RückmeldungsId, int[] Instruments, int[] Notenstimme, string Vorname, string Nachname, Rückmeldungsart Zugesagt, string? KommentarZusage, DateTime? LetzteRückmeldung, bool IstAnwesend, string? KommentarAnwesenheit, string? VornameRückmelder, string? NachnameRückmelder );
}

