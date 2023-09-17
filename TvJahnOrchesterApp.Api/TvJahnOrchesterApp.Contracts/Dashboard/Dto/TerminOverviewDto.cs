using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Dashboard.Dto
{
    public record TerminOverviewDto(Guid TerminId, string Name, TerminArt TerminArt, DateTime StartZeit);
}
