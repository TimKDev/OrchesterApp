using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.Einsatzplan.Endpoints
{
    public static class UpdateEinsatzplan
    {
        public static void MapUpdateEinsatzplanEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/einsatzPlan/{terminId}", UpdateUpdateEinsatzplan)
                .RequireAuthorization();
        }

        private static async Task<IResult> UpdateUpdateEinsatzplan([FromBody] EinsatzplanUpdateCommand einsatzplanUpdateCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(einsatzplanUpdateCommand, cancellationToken);
            return Results.Ok("Einsatzplan wurrde erfolgreich geupdated.");
        }

        private record EinsatzplanUpdateCommand(Guid TerminId, DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, int[] Noten, int[] Uniform, string? WeitereInformationen) : IRequest<Unit>;

        private class EinsatzplanUpdateCommandHandler : IRequestHandler<EinsatzplanUpdateCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public EinsatzplanUpdateCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(EinsatzplanUpdateCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var adresse = Adresse.Create(request.TreffPunkt.Straße, request.TreffPunkt.Hausnummer, request.TreffPunkt.Postleitzahl, request.TreffPunkt.Stadt);
                termin.EinsatzPlan.UpdateEinsatzPlan(request.StartZeit, request.EndZeit, adresse, request.WeitereInformationen);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return new Unit();
            }
        }
    }
}
