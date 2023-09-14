namespace TvJahnOrchesterApp.Application.Termin.Common
{
    public record UpdateAnwesenheitsListeResponse(UpdateAnwesenheitsEintrag[] UpdateAnwesenheitsListe); 
    public record UpdateAnwesenheitsEintrag(string Vorname, string Nachname, Guid OrchesterMitgliedsId, bool Anwesend, string? Kommentar);
}
