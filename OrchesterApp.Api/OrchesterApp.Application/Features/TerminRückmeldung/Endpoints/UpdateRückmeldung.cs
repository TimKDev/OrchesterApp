using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using static TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints.CreateOrchesterMitglied;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints
{
    public static class UpdateRückmeldung
    {
        public static void MapUpdateRückmeldungEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/rückmeldung", UpdateTerminRückmeldung)
                .RequireAuthorization();
        }

        private static async Task<IResult> UpdateTerminRückmeldung([FromBody] RückmeldungCommand rückmeldungCommand,
            ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(rückmeldungCommand, cancellationToken);

            return Results.Ok("Rückmeldung erfolgreich gespeichert.");
        }

        private record RückmeldungCommand(Guid TerminId, int Zugesagt, string? Kommentar) : IRequest<Unit>;

        private class RückmeldungCommandHandler : IRequestHandler<RückmeldungCommand, Unit>
        {
            private readonly ICurrentUserService currentUserService;
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public RückmeldungCommandHandler(ICurrentUserService currentUserService, ITerminRepository terminRepository,
                IUnitOfWork unitOfWork)
            {
                this.currentUserService = currentUserService;
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(RückmeldungCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var currentOrchesterMitglied =
                    await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                termin.RückmeldenZuTermin(currentOrchesterMitglied.Id, request.Zugesagt, request.Kommentar);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}