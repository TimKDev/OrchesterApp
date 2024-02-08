using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Endpoints
{
    public static class GetAllAnwesenheitsListe
    {
        public static void MapGetAllAnwesenheitsListeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/anwesenheit/all/{year}", GetAnwesenheitsListe)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetAnwesenheitsListe(int year, ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAllAnwesenheitsListeQuery(year), cancellationToken);
            return Results.Ok(response);
        }

        private record GlobalAnwesenheitsListenEintrag(Guid OrchesterMitgliedsId, int Ranking, string Name, int anwesendeTermin, int totalTermine, int AnwesendProzent);

        private record GlobalAnwesenheitsEintragWithoutRanking(Guid OrchesterMitgliedsId, string Name, int anwesendeTermin, int totalTermine, int AnwesendProzent);

        private record GetAllAnwesenheitsListeQuery(int Year) : IRequest<GlobalAnwesenheitsListenEintrag[]>;

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
                var terminsOfYear = allTermins.Where(t => t.EinsatzPlan.EndZeit.Year == request.Year);
                Dictionary<OrchesterMitgliedsId, (int numberAnwesendeTermine, int totalNumberTermine)> orchesterMitgliedsInfo = new Dictionary<OrchesterMitgliedsId, (int numberAnwesendeTermine, int totalNumberTermine)>();
                foreach(var termin in terminsOfYear)
                {
                    //Fill info
                }

                var resultWithoutRanking = new List<GlobalAnwesenheitsEintragWithoutRanking>();
                var orchesterMitglieder = await _orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                foreach(var orchesterMitglied in orchesterMitglieder)
                {
                    var info = orchesterMitgliedsInfo![orchesterMitglied.Id];
                    var percentValue = ;
                    resultWithoutRanking.Add(new GlobalAnwesenheitsEintragWithoutRanking(orchesterMitglied.Id.Value, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}", info.numberAnwesendeTermine, info.totalNumberTermine, percentValue));
                }


                return result.ToArray();
            }
        }

    }
}
