using MediatR;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockDelete
{
    public record EinsatzplanZeitblockDeleteCommand(Guid TerminId, Guid ZeitBlockId): IRequest<bool>;
}
