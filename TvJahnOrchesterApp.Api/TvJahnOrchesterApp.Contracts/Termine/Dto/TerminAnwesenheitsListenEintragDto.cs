namespace TvJahnOrchesterApp.Contracts.Termine.Dto
{
    public record TerminAnwesenheitsListenEintragDto(string Vorname, string Nachname, Guid OrchesterMitgliedsId, bool Anwesend, string? Kommentar);
}
