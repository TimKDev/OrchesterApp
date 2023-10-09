using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class DeleteTermin
    {
        public static void MapDeleteTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/termin/delete/{id}", DeleteTerminById)
                .RequireAuthorization();
        }

        private static async Task<IResult> DeleteTerminById(Guid id, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(new DeleteTerminCommand(id));
            return Results.Ok("Termin wurde gelöscht.");
        }

        private record DeleteTerminCommand(Guid Id) : IRequest<Unit>;

        private class DeleteTerminCommandHandler : IRequestHandler<DeleteTerminCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;

            public DeleteTerminCommandHandler(ITerminRepository terminRepository)
            {
                this.terminRepository = terminRepository;
            }

            public async Task<Unit> Handle(DeleteTerminCommand request, CancellationToken cancellationToken)
            {
                await terminRepository.Delete(request.Id, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
