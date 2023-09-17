using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockUpdate
{
    public record EinsatzplanZeitblockUpdateCommand(Guid TerminId, Guid? ZeitblockId, DateTime StartZeit, DateTime EndZeit, AdresseDto? Adresse, string Beschreibung): IRequest<Unit>;
}
