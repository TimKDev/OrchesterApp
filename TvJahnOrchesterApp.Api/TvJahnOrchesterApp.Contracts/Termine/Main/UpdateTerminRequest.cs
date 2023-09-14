using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record UpdateTerminRequest(Guid TerminId, string Name, TerminArt TerminArt, Guid[]? OrchestermitgliedIds);
}
