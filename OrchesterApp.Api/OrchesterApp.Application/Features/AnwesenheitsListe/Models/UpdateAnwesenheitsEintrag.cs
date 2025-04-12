namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Models
{
    public record UpdateAnwesenheitsEintrag(string Vorname, string Nachname, Guid OrchesterMitgliedsId, bool Anwesend, string? Kommentar);
}
