using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record UpdateTerminResponse(Guid TerminId, string Name, TerminArt TerminArt, Guid[]? OrchestermitgliedIds);
}
