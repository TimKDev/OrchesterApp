namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record GetAllTerminResponse(Guid TerminId, string Name, int TerminArt, DateTime StartZeit, DateTime EndZeit, bool IstAnwesend, int Zugesagt);
}
