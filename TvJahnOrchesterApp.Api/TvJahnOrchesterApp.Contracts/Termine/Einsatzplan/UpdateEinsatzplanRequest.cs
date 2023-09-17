using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateEinsatzplanRequest(DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, Noten[] Noten, Uniform[] Uniform, string? WeitereInformationen);
}
