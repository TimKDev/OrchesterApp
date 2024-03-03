using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualBasic;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class ConfirmEmail
    {
        public static void MapConfirmEmailEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/confirm-email", PostConfirmEmail)
                .RequireAuthorization();
        }

        private static async Task<IResult> PostConfirmEmail([FromBody] ConfirmEmailCommand confirmEmailCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(confirmEmailCommand);
            return Results.Ok("User Email wurde erfolgreich verifiziert.");
        }

        private record ConfirmEmailCommand(string Email, string Token) : IRequest<Unit>;

        private class ChangeUserEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
        {
            private readonly UserManager<User> userManager;

            public ChangeUserEmailCommandHandler(UserManager<User> userManager)
            {
                this.userManager = userManager;
            }

            public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var confirmResult = await userManager.ConfirmEmailAsync(user, request.Token);
                if (!confirmResult.Succeeded)
                {
                    throw new Exception("Invalid Email Confirmation Request");
                }

                return Unit.Value;
            }
        }
    }
}
