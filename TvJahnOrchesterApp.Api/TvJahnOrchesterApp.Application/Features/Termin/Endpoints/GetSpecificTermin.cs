using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class GetSpecificTermin
    {
        public static void MapGetSpecificTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Termin/getById/{id}", GetTerminById)
                .RequireAuthorization();
        }

        public static async Task<IResult> GetTerminById(Guid id, ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetTerminByIdQuery(id), cancellationToken);
            return Results.Ok(response);
        }

        public record GetTerminByIdQuery(Guid Id) : IRequest<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)>;

        public class GetTerminByIdQueryHandler : IRequestHandler<GetTerminByIdQuery, (Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;

            public GetTerminByIdQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
            }

            public async Task<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)> Handle(GetTerminByIdQuery request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.Id, cancellationToken);
                var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var currrentUserRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r => r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);
                if (currrentUserRückmeldung is null)
                {
                    throw new Exception("Throw custom exception");
                }

                return (termin, currrentUserRückmeldung!);
            }
        }

    }
}
