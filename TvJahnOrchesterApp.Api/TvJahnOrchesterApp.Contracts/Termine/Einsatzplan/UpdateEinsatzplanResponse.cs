using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateEinsatzplanResponse(DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, int[] Noten, int[] Uniform);
}
