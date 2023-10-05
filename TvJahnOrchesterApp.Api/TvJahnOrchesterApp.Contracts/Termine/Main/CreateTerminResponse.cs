namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record CreateTerminResponse(Guid TerminId, string Name, int TerminArt, Guid[]? OrchestermitgliedIds);
}
