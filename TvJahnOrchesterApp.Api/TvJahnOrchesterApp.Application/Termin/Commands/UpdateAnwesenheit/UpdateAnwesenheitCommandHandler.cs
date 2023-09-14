using MediatR;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Commands.UpdateAnwesenheit
{
    internal class UpdateAnwesenheitCommandHandler : IRequestHandler<UpdateAnwesenheitCommand, UpdateAnwesenheitsListeResponse>
    {
        public Task<UpdateAnwesenheitsListeResponse> Handle(UpdateAnwesenheitCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
