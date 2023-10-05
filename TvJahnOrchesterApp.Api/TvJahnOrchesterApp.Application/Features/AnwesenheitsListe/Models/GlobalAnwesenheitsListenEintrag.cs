namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Endpoints
{
    public record GlobalAnwesenheitsListenEintrag(string Vorname, string Nachname, Guid OrchesterMitgliedsId, string TerminName, bool Anwesend, DateTime TerminDate);
}
