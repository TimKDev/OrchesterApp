using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Create
{
    public record CreateTerminCommand(string Name, TerminArt TerminArt, DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, Guid[]? OrchestermitgliedIds) : IRequest<Domain.TerminAggregate.Termin>;
}
