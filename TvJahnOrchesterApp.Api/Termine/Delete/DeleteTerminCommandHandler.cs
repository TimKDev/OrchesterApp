using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Delete
{
    public class DeleteTerminCommandHandler : IRequestHandler<DeleteTerminCommand, bool>
    {
        private readonly ITerminRepository terminRepository;

        public DeleteTerminCommandHandler(ITerminRepository terminRepository)
        {
            this.terminRepository = terminRepository;
        }

        public async Task<bool> Handle(DeleteTerminCommand request, CancellationToken cancellationToken)
        {
            return await terminRepository.Delete(request.Id, cancellationToken);

        }
    }
}
