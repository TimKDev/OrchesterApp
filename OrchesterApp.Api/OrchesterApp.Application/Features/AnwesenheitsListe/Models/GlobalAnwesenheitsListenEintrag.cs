namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Models
{
    public record GlobalAnwesenheitsListenEintrag(string Vorname, string Nachname, Guid OrchesterMitgliedsId, string TerminName, bool Anwesend, DateTime TerminDate);
}
