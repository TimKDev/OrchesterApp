namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungTerminResponse(Guid TerminId, bool Zugesagt, string? Kommentar, string Vorname, string Nachname, Guid OrchesterMitgliedsId);
}
