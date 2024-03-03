using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using static TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints.CreateOrchesterMitglied;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints
{
    public static class UpdateAnwesenheitsListe
    {
        public static void MapUpdateAnwesenheitsListeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/anwesenheit", UpdateTerminAnwesenheitsListe)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand }));
        }

        private static async Task<IResult> UpdateTerminAnwesenheitsListe([FromBody] UpdateAnwesenheitsCommand updateAnwesenheitsCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(updateAnwesenheitsCommand, cancellationToken);

            return Results.Ok("Anwesenheit erfolgreich gespeichert.");
        }

        private record UpdateAnwesenheitsCommand(Guid TerminId, AnwesenheitResponseEntry[] AnwesenheitResponseEntries) : IRequest<Unit>;
        private record AnwesenheitResponseEntry(Guid OrchesterMitgliedsId, bool IstAnwesend, string? KommentarAnwesenheit);

        private class UpdateAnwesenheitsCommandHandler : IRequestHandler<UpdateAnwesenheitsCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public UpdateAnwesenheitsCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(UpdateAnwesenheitsCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                foreach(var anwesenheit in request.AnwesenheitResponseEntries)
                {
                    termin.AnwesenheitZuTermin(OrchesterMitgliedsId.Create(anwesenheit.OrchesterMitgliedsId), anwesenheit.IstAnwesend, anwesenheit.KommentarAnwesenheit);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

    }
}
