using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record CreateTerminRequest(string Name, int TerminArt, DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, Guid[]? OrchestermitgliedIds, int[] Noten, int[] Uniform);
}
