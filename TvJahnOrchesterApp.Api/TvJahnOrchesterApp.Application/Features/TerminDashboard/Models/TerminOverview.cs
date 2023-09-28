using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Application.Features.TerminDashboard
{
    public static partial class GetNextTermins
    {
        public record TerminOverview(Guid TerminId, string Name, TerminArt TerminArt, DateTime StartZeit);
    }
}
