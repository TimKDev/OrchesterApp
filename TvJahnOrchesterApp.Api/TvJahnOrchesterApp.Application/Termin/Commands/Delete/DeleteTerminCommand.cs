using MediatR;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Delete
{
    public record DeleteTerminCommand(Guid Id): IRequest<bool>;
}
