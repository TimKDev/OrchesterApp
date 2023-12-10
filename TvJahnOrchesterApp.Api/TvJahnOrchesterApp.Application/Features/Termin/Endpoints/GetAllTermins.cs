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
    public static class GetAllTermins
    {
        public static void MapGetAllTerminsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/all", GetGetAllTermins)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetGetAllTermins(ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAllTermineQuery(), cancellationToken);
            return Results.Ok(response);
        }

        private record GetAllTermineQuery() : IRequest<GetAllTermineResponse[]>;

        private record GetAllTermineResponse(Guid TerminId, string Name, int? TerminArt, int? TerminStatus, DateTime StartZeit, DateTime EndZeit, int Zugesagt, bool IstAnwesend, int NoResponse, int PositiveResponse, int NegativeResponse);

        private class GetAllTermineQueryHandler : IRequestHandler<GetAllTermineQuery, GetAllTermineResponse[]>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;

            public GetAllTermineQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
            }

            public async Task<GetAllTermineResponse[]> Handle(GetAllTermineQuery request, CancellationToken cancellationToken)
            {
                var result = new List<GetAllTermineResponse>();
                var termins = await terminRepository.GetAll(cancellationToken);
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

                    var terminEntry = new GetAllTermineResponse(termin.Id.Value, termin.Name, termin.TerminArt, termin.TerminStatus, termin.EinsatzPlan.StartZeit, termin.EinsatzPlan.EndZeit, currrentUserRückmeldung?.Zugesagt ?? (int)RückmeldungsartEnum.NichtZurückgemeldet, currrentUserRückmeldung?.IstAnwesend ?? false, countNoResponse, countPositiveResponse, countNegativeResponse);

                    result.Add(terminEntry);
                }

                return result.OrderByDescending(r => r.StartZeit).ToArray();
            }
        }
    }
}
