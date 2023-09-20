using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockDelete
{
    internal class EinsatzplanZeitblockDeleteCommandHandler : IRequestHandler<EinsatzplanZeitblockDeleteCommand, bool>
    {
        private readonly ITerminRepository terminRepository;

        public EinsatzplanZeitblockDeleteCommandHandler(ITerminRepository terminRepository)
        {
            this.terminRepository = terminRepository;
        }

        public async Task<bool> Handle(EinsatzplanZeitblockDeleteCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            termin.EinsatzPlan.DeleteZeitBlock(ZeitblockId.Create( request.ZeitBlockId));

            return true;
        }
    }
}
