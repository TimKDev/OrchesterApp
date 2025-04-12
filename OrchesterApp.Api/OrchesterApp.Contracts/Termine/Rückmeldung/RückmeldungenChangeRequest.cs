namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungenChangeRequest(Guid RückmeldungsId, int[] Instruments, int[] Notenstimme);
}
