namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungTerminRequest(Guid TerminId, bool Zugesagt, string? Kommentar);
}
