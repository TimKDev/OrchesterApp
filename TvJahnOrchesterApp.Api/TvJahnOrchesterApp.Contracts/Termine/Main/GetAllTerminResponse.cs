using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record GetAllTerminResponse(Guid TerminId, string Name, TerminArt TerminArt, DateTime StartZeit, DateTime EndZeit, bool IstAnwesend, Rückmeldungsart Zugesagt);
}
