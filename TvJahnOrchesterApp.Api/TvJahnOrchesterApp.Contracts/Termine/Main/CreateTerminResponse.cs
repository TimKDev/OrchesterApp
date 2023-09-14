using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record CreateTerminResponse(Guid TerminId, string Name, TerminArt TerminArt, Guid[]? OrchestermitgliedIds);
}
