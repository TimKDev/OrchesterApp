using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
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
            return Results.Ok(response.Select(c => new GetAllTermineResponse(c.Item1, c.Item2)));
        }

        private record GetAllTermineQuery() : IRequest<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied?)[]>;

        private record GetAllTermineResponse(Domain.TerminAggregate.Termin Termin, TerminRückmeldungOrchestermitglied TerminRückmeldung);

        private class GetAllTermineQueryHandler : IRequestHandler<GetAllTermineQuery, (Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied?)[]>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;

            public GetAllTermineQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
            }

            public async Task<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)[]> Handle(GetAllTermineQuery request, CancellationToken cancellationToken)
            {
                var result = new List<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)>();
                var termins = await terminRepository.GetAll(cancellationToken);
                foreach (var termin in termins)
                {
                    var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                    var currrentUserRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r => r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);
                    result.Add((termin, currrentUserRückmeldung ?? TerminRückmeldungOrchestermitglied.Create(OrchesterMitgliedsId.CreateUnique(), new List<int?>(), new List<int?>())));
                }

                return result.ToArray();
            }
        }
    }
}
