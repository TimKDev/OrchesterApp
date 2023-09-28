using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanUpdate
{
    internal class EinsatzplanUpdateCommandHandler : IRequestHandler<EinsatzplanUpdateCommand, Unit>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IUnitOfWork unitOfWork;

        public EinsatzplanUpdateCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
        {
            this.terminRepository = terminRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(EinsatzplanUpdateCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var adresse = Adresse.Create(request.TreffPunkt.Straße, request.TreffPunkt.Hausnummer, request.TreffPunkt.Postleitzahl, request.TreffPunkt.Stadt);
            termin.EinsatzPlan.UpdateEinsatzPlan(request.StartZeit, request.EndZeit, adresse, request.Noten.Select(Noten.Create).ToArray(), request.Uniform.Select(Uniform.Create).ToArray(), request.WeitereInformationen);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new Unit();
        }
    }
}
