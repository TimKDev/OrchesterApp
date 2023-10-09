using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.Einsatzplan.Endpoints
{
    public static class UpdateZeitblock
    {
        public static void MapUpdateZeitblockEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/termin/einsatzPlan/{terminId}/zeitblock", UpdateEinsatzplanZeitblock)
                .RequireAuthorization();
        }

        private static async Task<IResult> UpdateEinsatzplanZeitblock(Guid terminId, [FromBody] EinsatzplanZeitblockUpdateCommand einsatzplanZeitblockUpdateCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(einsatzplanZeitblockUpdateCommand, cancellationToken);
            return Results.Ok("Zeitblock wurde erfolgreich geupdated.");
        }

        private record EinsatzplanZeitblockUpdateCommand(Guid TerminId, Guid? ZeitblockId, DateTime StartZeit, DateTime EndZeit, AdresseDto? Adresse, string Beschreibung) : IRequest<Unit>;

        private class EinsatzplanZeitblockUpdateCommandHandler : IRequestHandler<EinsatzplanZeitblockUpdateCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public EinsatzplanZeitblockUpdateCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(EinsatzplanZeitblockUpdateCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var adresse = request.Adresse is not null ? Adresse.Create(request.Adresse.Straße, request.Adresse.Hausnummer, request.Adresse.Postleitzahl, request.Adresse.Stadt) : null;
                if (request.ZeitblockId is null)
                {
                    termin.EinsatzPlan.AddZeitBlock(request.StartZeit, request.EndZeit, request.Beschreibung, adresse);
                }
                else
                {
                    termin.EinsatzPlan.UpdateZeitBlock(ZeitblockId.Create((Guid)request.ZeitblockId), request.StartZeit, request.EndZeit, request.Beschreibung, adresse);
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return new Unit();

            }
        }
    }
}
