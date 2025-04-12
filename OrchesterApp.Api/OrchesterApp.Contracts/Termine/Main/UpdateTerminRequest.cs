namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record UpdateTerminRequest(Guid TerminId, string Name, int TerminArt, Guid[]? OrchestermitgliedIds);
}
