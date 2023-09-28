using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockDelete
{
    internal class EinsatzplanZeitblockDeleteCommandHandler : IRequestHandler<EinsatzplanZeitblockDeleteCommand, bool>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IUnitOfWork unitOfWork;

        public EinsatzplanZeitblockDeleteCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
        {
            this.terminRepository = terminRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(EinsatzplanZeitblockDeleteCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            termin.EinsatzPlan.DeleteZeitBlock(ZeitblockId.Create( request.ZeitBlockId));
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
