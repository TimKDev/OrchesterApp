namespace TvJahnOrchesterApp.Contracts.Termine.AnwesenheitsListe
{
    public record GetAnwesenheitsListeResponse(GlobalAnwesenheitsListenEintrag[] GlobalAnwesenheitsListe);

    public record GlobalAnwesenheitsListenEintrag(string Vorname, string Nachname, Guid OrchesterMitgliedsId, string TerminName, bool Anwesend, DateTime TerminDate);
}
