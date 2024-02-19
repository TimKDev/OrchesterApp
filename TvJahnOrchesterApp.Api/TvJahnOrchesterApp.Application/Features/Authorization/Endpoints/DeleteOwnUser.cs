using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class DeleteOwnUser
    {
        public static void MapDeleteOwnUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/authentication/delete-own-user", DeleteDeleteOwnUser)
                .RequireAuthorization();
        }

        private static async Task<IResult> DeleteDeleteOwnUser(CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(new DeleteOwnUserCommand());
            return Results.Ok("User erfolgreich gelöscht");
        }

        private record DeleteOwnUserCommand() : IRequest<Unit>;

        private class DeleteOwnUserCommandHandler : IRequestHandler<DeleteOwnUserCommand, Unit>
        {
            private readonly UserManager<User> userManager;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly ICurrentUserService currentUserService;
            private readonly IUnitOfWork unitOfWork;

            public DeleteOwnUserCommandHandler(UserManager<User> userManager, IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
            {
                this.userManager = userManager;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
                this.currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(DeleteOwnUserCommand request, CancellationToken cancellationToken)
            {
                var user = await currentUserService.GetCurrentUserAsync(cancellationToken);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var orchesterMitglied = await orchesterMitgliedRepository.GetByUserIdAsync(user.Id, cancellationToken);
                if (orchesterMitglied is null)
                {
                    throw new Exception("Orchestermitglied nicht gefunden");
                }

                orchesterMitglied.RemoveUser();
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await userManager.DeleteAsync(user);

                return Unit.Value;
            }
        }
    }
}
