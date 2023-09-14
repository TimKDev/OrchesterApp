using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.Enums;
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

        public UpdateTerminCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
        }

        public async Task<Domain.TerminAggregate.Termin> Handle(UpdateTerminCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            //TTODO Throw Exception when Termin is not found!
            termin.UpdateName(request.Name);
            termin.UpdateTerminArt(request.TerminArt);

            if (request.OrchestermitgliedIds is null) return termin;

            var orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdAsync(request.OrchestermitgliedIds.Select(id => OrchesterMitgliedsId.Create(id)).ToArray(), cancellationToken);

            var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o => TerminRückmeldungOrchestermitglied.Create(o.Id, new List<Instrument> { o.DefaultInstrument }, new List<Notenstimme> { o.DefaultNotenStimme })).ToArray();

            termin.UpdateTerminRückmeldungOrchestermitglied(terminRückmeldungOrchesterMitglieder);

            return termin;
        }
    }
}
