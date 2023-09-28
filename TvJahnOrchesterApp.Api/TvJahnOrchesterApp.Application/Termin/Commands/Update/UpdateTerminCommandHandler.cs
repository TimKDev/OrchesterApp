using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Update
{
    public class UpdateTerminCommandHandler : IRequestHandler<UpdateTerminCommand, Domain.TerminAggregate.Termin>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateTerminCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Domain.TerminAggregate.Termin> Handle(UpdateTerminCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            //TTODO Throw Exception when Termin is not found!
            termin.UpdateName(request.Name);
            termin.UpdateTerminArt(request.TerminArt);

            if (request.OrchestermitgliedIds is null) return termin;

            var orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdAsync(request.OrchestermitgliedIds.Select(OrchesterMitgliedsId.Create).ToArray(), cancellationToken);

            var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o => TerminRückmeldungOrchestermitglied.Create(o.Id, new List<Instrument> { o.DefaultInstrument }, new List<NotenstimmeEnum> { o.DefaultNotenStimme.Stimme })).ToArray();

            termin.UpdateTerminRückmeldungOrchestermitglied(terminRückmeldungOrchesterMitglieder);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return termin;
        }
    }
}
