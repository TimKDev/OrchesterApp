using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class GetAllTerminsHistory
    {
        public static void MapGetAllTerminsHistoryEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/all/history", GetGetAllTerminsHistory)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetGetAllTerminsHistory(ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAllTerminsHistoryQuery(), cancellationToken);
            return Results.Ok(response);
        }

        private record GetAllTerminsHistoryQuery() : IRequest<GetAllTerminsHistoryResponse[]>;

        private record GetAllTerminsHistoryResponse(Guid TerminId, string Name, int? TerminArt, int? TerminStatus, DateTime StartZeit, DateTime EndZeit, int Zugesagt, bool IstAnwesend, int NoResponse, int PositiveResponse, int NegativeResponse);

        private class GetAllTerminsHistoryQueryHandler : IRequestHandler<GetAllTerminsHistoryQuery, GetAllTerminsHistoryResponse[]>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;

            public GetAllTerminsHistoryQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
            }

            public async Task<GetAllTerminsHistoryResponse[]> Handle(GetAllTerminsHistoryQuery request, CancellationToken cancellationToken)
            {
                var result = new List<GetAllTerminsHistoryResponse>();
                var termins = (await terminRepository.GetAll(cancellationToken)).Where(t => t.EinsatzPlan.EndZeit < DateTime.Now);
                foreach (var termin in termins)
                {
                    var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                    var currrentUserRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r => r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);

                    var countNoResponse = 0;
                    var countPositiveResponse = 0;
                    var countNegativeResponse = 0;
                    foreach(var rückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
                    {
                        if(rückmeldung.Zugesagt == (int)RückmeldungsartEnum.NichtZurückgemeldet)
                        {
                            countNoResponse++;
                        }
                        if(rückmeldung.Zugesagt == (int)RückmeldungsartEnum.Zugesagt)
                        {
                            countPositiveResponse++;
                        }
                        if (rückmeldung.Zugesagt == (int)RückmeldungsartEnum.Abgesagt)
                        {
                            countNegativeResponse++;
                        }
                    }

                    var terminEntry = new GetAllTerminsHistoryResponse(termin.Id.Value, termin.Name, termin.TerminArt, termin.TerminStatus, termin.EinsatzPlan.StartZeit, termin.EinsatzPlan.EndZeit, currrentUserRückmeldung?.Zugesagt ?? (int)RückmeldungsartEnum.NichtZurückgemeldet, currrentUserRückmeldung?.IstAnwesend ?? false, countNoResponse, countPositiveResponse, countNegativeResponse);

                    result.Add(terminEntry);
                }

                return result.OrderByDescending(r => r.StartZeit).ToArray();
            }
        }
    }
}
