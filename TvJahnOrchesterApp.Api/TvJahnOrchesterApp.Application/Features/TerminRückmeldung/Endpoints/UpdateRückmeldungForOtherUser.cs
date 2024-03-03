using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using static TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints.CreateOrchesterMitglied;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints
{
    public static class UpdateRückmeldungForOtherUser
    {
        public static void MapUpdateRückmeldungForOtherUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/rückmeldung/forUser", UpdateTerminRückmeldungForUser)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand }));
        }

        private static async Task<IResult> UpdateTerminRückmeldungForUser([FromBody] RückmeldungForOtherUserCommand rückmeldungForOtherUserCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(rückmeldungForOtherUserCommand, cancellationToken);

            return Results.Ok("Rückmeldung für anderen User wurde erfolgreich gespeichert.");
        }

        private record RückmeldungForOtherUserCommand(Guid TerminId, Guid OrchesterMitgliedsId, int Zugesagt, string? KommentarZusage, bool IstAnwesend, string? KommentarAnwesenheit) : IRequest<Unit>;

        private class RückmeldungForOtherUserCommandHandler : IRequestHandler<RückmeldungForOtherUserCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;

            public RückmeldungForOtherUserCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.currentUserService = currentUserService;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(RückmeldungForOtherUserCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.OrchesterMitgliedsId), cancellationToken);
                var currentOrchestermitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);

                termin.RückmeldenZuTermin(orchesterMitglied.Id, request.Zugesagt, request.KommentarZusage, currentOrchestermitglied.Id);
                termin.AnwesenheitZuTermin(orchesterMitglied.Id, request.IstAnwesend, request.KommentarAnwesenheit);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

    }
}
