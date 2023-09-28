using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanUpdate
{
    public record EinsatzplanUpdateCommand(Guid TerminId, DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, NotenEnum[] Noten, UniformEnum[] Uniform, string? WeitereInformationen): IRequest<Unit>;
}
