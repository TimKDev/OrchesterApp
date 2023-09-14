using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetSpecific
{
    public class GetTerminByIdQueryHandler : IRequestHandler<GetTerminByIdQuery, TerminResponse>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

        public GetTerminByIdQueryHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
        }

        public async Task<TerminResponse> Handle(GetTerminByIdQuery request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.Id, cancellationToken);
            var terminRückmeldungOrchestermitglieder = new List<(string Vorname, string Nachname, string? VornameOther, string? NachnameOther, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied)>();
            foreach(var terminRückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(terminRückmeldung.OrchesterMitgliedsId, cancellationToken);
                if(terminRückmeldung.RückmeldungDurchAnderesOrchestermitglied is not null)
                {
                   var otherOrchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(terminRückmeldung.RückmeldungDurchAnderesOrchestermitglied, cancellationToken);
                    terminRückmeldungOrchestermitglieder.Add(new(orchesterMitglied.Vorname, orchesterMitglied.Nachname, otherOrchesterMitglied.Vorname, otherOrchesterMitglied.Nachname, terminRückmeldung));

                }
                else
                {
                    terminRückmeldungOrchestermitglieder.Add(new(orchesterMitglied.Vorname, orchesterMitglied.Nachname, null, null, terminRückmeldung));
                }
            }

            return new TerminResponse(termin, terminRückmeldungOrchestermitglieder.ToArray());
        }
    }
}
