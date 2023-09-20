using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanUpdate
{
    internal class EinsatzplanUpdateCommandHandler : IRequestHandler<EinsatzplanUpdateCommand, Unit>
    {
        private readonly ITerminRepository terminRepository;

        public EinsatzplanUpdateCommandHandler(ITerminRepository terminRepository)
        {
            this.terminRepository = terminRepository;
        }

        public async Task<Unit> Handle(EinsatzplanUpdateCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var adresse = Adresse.Create(request.TreffPunkt.Straße, request.TreffPunkt.Hausnummer, request.TreffPunkt.Postleitzahl, request.TreffPunkt.Stadt);
            termin.EinsatzPlan.UpdateEinsatzPlan(request.StartZeit, request.EndZeit, adresse, request.Noten, request.Uniform, request.WeitereInformationen);

            return new Unit();
        }
    }
}
