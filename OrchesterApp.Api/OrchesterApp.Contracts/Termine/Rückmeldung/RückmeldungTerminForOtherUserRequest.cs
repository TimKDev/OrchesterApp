namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungTerminForOtherUserRequest(Guid TerminId, Guid OrchesterMitgliedsId, bool Zugesagt, string? Kommentar);
}
