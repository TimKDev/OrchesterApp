using MediatR;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Update
{
    public record UpdateTerminCommand(Guid TerminId, string Name, TerminArt TerminArt, Guid[]? OrchestermitgliedIds) : IRequest<Domain.TerminAggregate.Termin>;
}
