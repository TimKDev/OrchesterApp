using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Contracts.Termine.AnwesenheitsListe;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAllAnwesenheitsListe
{
    internal class GetAllAnwesenheitsListeQueryHandler : IRequestHandler<GetAllAnwesenheitsListeQuery, GlobalAnwesenheitsListe>
    {
        private readonly IOrchesterMitgliedRepository _orchesterMitgliedRepository;
        private readonly ITerminRepository _terminRepository;

        public GetAllAnwesenheitsListeQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, ITerminRepository terminRepository)
        {
            _orchesterMitgliedRepository = orchesterMitgliedRepository;
            _terminRepository = terminRepository;
        }

        public async Task<GlobalAnwesenheitsListe> Handle(GetAllAnwesenheitsListeQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GlobalAnwesenheitsListenEintrag>();
            var allTermins = await _terminRepository.GetAll(cancellationToken);
            foreach (var termin in allTermins) 
            { 
                foreach(var rückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
                {
                    // Sehr ineffizient: Dieselben Daten werden mehrfach aus der DB geholt, Orchestermitglieder könnten hier gecached werden oder zumindest über Navigation Properties eingebunden werden:
                    var orchesterMitglied = await _orchesterMitgliedRepository.GetByIdAsync(rückmeldung.OrchesterMitgliedsId, cancellationToken);
                    result.Add(new GlobalAnwesenheitsListenEintrag(
                        orchesterMitglied.Vorname,
                        orchesterMitglied.Nachname,
                        orchesterMitglied.Id.Value,
                        termin.Name,
                        rückmeldung.IstAnwesend,
                        termin.EinsatzPlan.StartZeit
                    ));
                }
            }
            return new GlobalAnwesenheitsListe(result.ToArray());
        }
    }
}
