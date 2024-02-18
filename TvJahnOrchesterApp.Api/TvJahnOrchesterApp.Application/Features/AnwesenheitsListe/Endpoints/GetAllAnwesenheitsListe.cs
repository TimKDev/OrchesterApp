using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using TvJahnOrchesterApp.Application.Common.Interfaces.Dto;
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

        private record GlobalAnwesenheitsListenEintrag(Guid OrchesterMitgliedsId, int Ranking, string Name, int AnwesendeTermin, int TotalTermine, int AnwesendProzent);

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

                var orchesterMitglieder = await _orchesterMitgliedRepository.GetAllNames(cancellationToken);
                Dictionary<OrchesterMitgliedsId, (int numberAnwesendeTermine, int totalNumberTermine)> orchesterMitgliedsInfo = await CalculateOrchesterTerminNumbersAsync(request.Year, orchesterMitglieder, cancellationToken);

                var resultWithoutRanking = new List<GlobalAnwesenheitsEintragWithoutRanking>();
                foreach(var orchesterMitglied in orchesterMitglieder)
                {
                    var info = orchesterMitgliedsInfo![orchesterMitglied.Id];
                    var percentValue = info.totalNumberTermine != 0 ? (int)Math.Round(((double)info.numberAnwesendeTermine / info.totalNumberTermine) * 100) : 0;
                    resultWithoutRanking.Add(new GlobalAnwesenheitsEintragWithoutRanking(orchesterMitglied.Id.Value, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}", info.numberAnwesendeTermine, info.totalNumberTermine, percentValue));
                }

                var orderedList = resultWithoutRanking.OrderByDescending(x => x.anwesendeTermin);
                var nextRank = 1;
                var lastValue = 0;
                foreach(var ordered in orderedList)
                {
                    if(lastValue != 0 && ordered.anwesendeTermin != lastValue)
                    {
                        nextRank++;
                    }
                    lastValue = ordered.anwesendeTermin;
                    result.Add(new GlobalAnwesenheitsListenEintrag(ordered.OrchesterMitgliedsId, nextRank, ordered.Name, ordered.anwesendeTermin, ordered.totalTermine, ordered.AnwesendProzent));
                }

                return result.ToArray();
            }

            private async Task<Dictionary<OrchesterMitgliedsId, (int numberAnwesendeTermine, int totalNumberTermine)>> CalculateOrchesterTerminNumbersAsync(int year, OrchesterMitgliedWithName[] orchesterMitglieder, CancellationToken cancellationToken)
            {
                Dictionary<OrchesterMitgliedsId, (int numberAnwesendeTermine, int totalNumberTermine)> result = new Dictionary<OrchesterMitgliedsId, (int numberAnwesendeTermine, int totalNumberTermine)>();

                var terminsOfYear = await _terminRepository.GetTerminResponsesInYear(year, cancellationToken);
                foreach (var orchesterMitglied in orchesterMitglieder)
                {
                    result.Add(orchesterMitglied.Id, (0, 0));
                }

                foreach (var termin in terminsOfYear)
                {
                    foreach (var terminResponse in termin.RückmeldungOrchestermitglieder)
                    {
                        if (result.TryGetValue(terminResponse.OrchesterMitgliedsId, out (int numberAnwesendeTermine, int totalNumberTermine) numbers))
                        {
                            result[terminResponse.OrchesterMitgliedsId] = (numbers.numberAnwesendeTermine + (terminResponse.IstAnwesend ? 1 : 0), numbers.totalNumberTermine + 1);
                        }
                    }
                }

                return result;
            }
        }

    }
}
