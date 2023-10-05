using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.Einsatzplan.Endpoints
{
    public static class DeleteZeitblock
    {
        public static void MapDeleteZeitblockEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/Termin/EinsatzPlan/{terminId}/Zeitblock/{zeitBlockId}", DeleteEinsatzplanZeitblock)
                .RequireAuthorization();
        }

        public static async Task<IResult> DeleteEinsatzplanZeitblock(Guid terminId, Guid zeitBlockId, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(new EinsatzplanZeitblockDeleteCommand(terminId, zeitBlockId), cancellationToken);
            return Results.Ok("Zeitblock wurde erfolgreich gelöscht.");
        }

        public record EinsatzplanZeitblockDeleteCommand(Guid TerminId, Guid ZeitBlockId) : IRequest<Unit>;

        internal class EinsatzplanZeitblockDeleteCommandHandler : IRequestHandler<EinsatzplanZeitblockDeleteCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public EinsatzplanZeitblockDeleteCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(EinsatzplanZeitblockDeleteCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                termin.EinsatzPlan.DeleteZeitBlock(ZeitblockId.Create(request.ZeitBlockId));
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }

    }
}
