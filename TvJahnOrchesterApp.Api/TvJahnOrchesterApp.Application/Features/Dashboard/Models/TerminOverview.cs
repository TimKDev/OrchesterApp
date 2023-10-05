namespace TvJahnOrchesterApp.Application.Features.TerminDashboard
{
    public static partial class GetNextTermins
    {
        public record TerminOverview(Guid TerminId, string Name, int? TerminArt, DateTime StartZeit);
    }
}
