using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateEinsatzplanRequest(DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, NotenEnum[] Noten, UniformEnum[] Uniform, string? WeitereInformationen);
}
