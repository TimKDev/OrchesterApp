namespace TvJahnOrchesterApp.Contracts.Termine.Dto
{
    public record AnwesenheitsListenEintragDto(string Vorname, string Nachname, Guid OrchesterMitgliedsId, bool Anwesend, string? Kommentar);
}
