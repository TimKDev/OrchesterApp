using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAnwesenheitsListe
{
    internal class GetAnwesenheitsListeQueryHandler : IRequestHandler<GetAnwesenheitsListeQuery, TerminAnwesenheitsListeResponse>
    {
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
        private readonly ITerminRepository terminRepository;

        public GetAnwesenheitsListeQueryHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
        }

        public async Task<TerminAnwesenheitsListeResponse> Handle(GetAnwesenheitsListeQuery request, CancellationToken cancellationToken)
        {
            var result = new List<TerminAnwesenheitsListenEintrag>();
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            foreach(Domain.TerminAggregate.Entities.TerminRückmeldungOrchestermitglied rückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(rückmeldung.OrchesterMitgliedsId, cancellationToken);
                result.Add(new TerminAnwesenheitsListenEintrag(orchesterMitglied.Vorname, orchesterMitglied.Nachname, rückmeldung));
            }
            return new TerminAnwesenheitsListeResponse(result.ToArray());
        }
    }
}
