using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockUpdate
{
    internal class EinsatzplanZeitblockUpdateCommandHandler : IRequestHandler<EinsatzplanZeitblockUpdateCommand, Unit>
    {
        private readonly ITerminRepository terminRepository;

        public EinsatzplanZeitblockUpdateCommandHandler(ITerminRepository terminRepository)
        {
            this.terminRepository = terminRepository;
        }

        public async Task<Unit> Handle(EinsatzplanZeitblockUpdateCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var adresse = request.Adresse is not null ? Adresse.Create(request.Adresse.Straße, request.Adresse.Hausnummer, request.Adresse.Postleitzahl, request.Adresse.Stadt) : null;
            if(request.ZeitblockId is null)
            {
                termin.EinsatzPlan.AddZeitBlock(request.StartZeit, request.EndZeit, request.Beschreibung, adresse);
            }
            else
            {
                termin.EinsatzPlan.UpdateZeitBlock(ZeitblockId.Create((Guid)request.ZeitblockId), request.StartZeit, request.EndZeit, request.Beschreibung, adresse);
            }
            return new Unit();

        }
    }
}
