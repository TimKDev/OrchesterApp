using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateEinsatzplanResponse(DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, Noten[] Noten, Uniform[] Uniform);
}
