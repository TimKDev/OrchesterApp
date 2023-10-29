using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class RevokeUserLockedOut
    {
        public static void MapRevokeUserLockedOutEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/remove-user-locked-out", PostRevokeUserLockedOut)
                .RequireAuthorization();
        }

        private static async Task<IResult> PostRevokeUserLockedOut([FromBody] RevokeUserLockedOutCommand revokeUserLockedOutCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(revokeUserLockedOutCommand);
            return Results.Ok("Accountsperre für User erfolgreich zurückgesetzt.");
        }

        private record RevokeUserLockedOutCommand(string UserId) : IRequest<Unit>;

        private class RevokeUserLockedOutCommandHandler : IRequestHandler<RevokeUserLockedOutCommand, Unit>
        {
            private readonly UserManager<User> userManager;

            public RevokeUserLockedOutCommandHandler(UserManager<User> userManager)
            {
                this.userManager = userManager;
            }

            public async Task<Unit> Handle(RevokeUserLockedOutCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByIdAsync(request.UserId);
                await userManager.ResetAccessFailedCountAsync(user);
                await userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

                return Unit.Value;
            }
        }
    }
}
