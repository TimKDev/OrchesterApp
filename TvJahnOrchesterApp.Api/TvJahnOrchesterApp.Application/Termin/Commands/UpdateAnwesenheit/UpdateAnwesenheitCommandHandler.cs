using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.UpdateAnwesenheit
{
    internal class UpdateAnwesenheitCommandHandler : IRequestHandler<UpdateAnwesenheitCommand, UpdateAnwesenheitsListeResponse>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateAnwesenheitCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
        {
            this.terminRepository = terminRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<UpdateAnwesenheitsListeResponse> Handle(UpdateAnwesenheitCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            foreach(var anwesenheitsElement in request.UpdateAnwesenheitsListe)
            {
                var terminRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(e => e.OrchesterMitgliedsId == OrchesterMitgliedsId.Create(anwesenheitsElement.OrchesterMitgliedsId));
                if(terminRückmeldung is null)
                {
                    continue;
                }
                terminRückmeldung.ChangeAnwesenheit(anwesenheitsElement.Anwesend, anwesenheitsElement.Kommentar);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new UpdateAnwesenheitsListeResponse(request.UpdateAnwesenheitsListe);
        }
    }
}
