namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record UpdateTerminResponse(Guid TerminId, string Name, int TerminArt, Guid[]? OrchestermitgliedIds);
}
