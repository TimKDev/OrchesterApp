using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record CreateTerminRequest(string Name, TerminArt TerminArt, DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, Guid[]? OrchestermitgliedIds, NotenEnum[] Noten, UniformEnum[] Uniform);
}
