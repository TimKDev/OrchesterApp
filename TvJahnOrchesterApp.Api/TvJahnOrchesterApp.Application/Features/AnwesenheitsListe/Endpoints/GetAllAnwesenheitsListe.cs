using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Endpoints
{
    public static class GetAllAnwesenheitsListe
    {
        public static void MapGetAllAnwesenheitsListeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/anwesenheit/all", GetAnwesenheitsListe)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetAnwesenheitsListe(ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAllAnwesenheitsListeQuery(), cancellationToken);
            return Results.Ok(response);
        }

        private record GetAllAnwesenheitsListeQuery() : IRequest<GlobalAnwesenheitsListenEintrag[]>;

        private class GetAllAnwesenheitsListeQueryHandler : IRequestHandler<GetAllAnwesenheitsListeQuery, GlobalAnwesenheitsListenEintrag[]>
        {
            private readonly IOrchesterMitgliedRepository _orchesterMitgliedRepository;
            private readonly ITerminRepository _terminRepository;

            public GetAllAnwesenheitsListeQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, ITerminRepository terminRepository)
            {
                _orchesterMitgliedRepository = orchesterMitgliedRepository;
                _terminRepository = terminRepository;
            }

            public async Task<GlobalAnwesenheitsListenEintrag[]> Handle(GetAllAnwesenheitsListeQuery request, CancellationToken cancellationToken)
            {
                var result = new List<GlobalAnwesenheitsListenEintrag>();
                var allTermins = await _terminRepository.GetAll(cancellationToken);
                foreach (var termin in allTermins)
                {
                    foreach (var rückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
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
                return result.ToArray();
            }
        }

    }
}
