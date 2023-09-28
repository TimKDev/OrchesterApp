using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Create
{
    public class CreateTerminCommandHandler : IRequestHandler<CreateTerminCommand, Domain.TerminAggregate.Termin>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

        public CreateTerminCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
        }

        public async Task<Domain.TerminAggregate.Termin> Handle(CreateTerminCommand request, CancellationToken cancellationToken)
        {
            Domain.OrchesterMitgliedAggregate.OrchesterMitglied[] orchesterMitglieder;
            if(request.OrchestermitgliedIds is not null && request.OrchestermitgliedIds.Length > 0)
            {
                orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdAsync(request.OrchestermitgliedIds.Select(id => OrchesterMitgliedsId.Create(id)).ToArray(), cancellationToken);
            }
            else
            {
                orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
            }

            var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o => TerminRückmeldungOrchestermitglied.Create(o.Id, new List<Instrument> { o.DefaultInstrument }, new List<NotenstimmeEnum> { o.DefaultNotenStimme.Stimme })).ToArray();

            var treffpunkt = Adresse.Create(request.TreffPunkt.Straße, request.TreffPunkt.Hausnummer, request.TreffPunkt.Postleitzahl, request.TreffPunkt.Stadt);

            var termin = Domain.TerminAggregate.Termin.Create(terminRückmeldungOrchesterMitglieder, request.Name, request.TerminArt, request.StartZeit, request.EndZeit, treffpunkt, request.Noten.ToList(), request.Uniform.ToList());

            return await terminRepository.Save(termin, cancellationToken);
        }
    }
}
