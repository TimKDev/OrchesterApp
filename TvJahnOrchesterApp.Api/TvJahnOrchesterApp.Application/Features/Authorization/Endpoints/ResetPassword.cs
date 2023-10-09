using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class ResetPassword
    {
        public static void MapResetPasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/resetPassword", PostResetPassword);
        }

        private static async Task<IResult> PostResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(resetPasswordCommand);
            return Results.Ok("Passwort wurde resetet.");
        }

        private record ResetPasswordCommand(string Email, string Password, string ResetPasswordToken) : IRequest<Unit>;

        private class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
        {
            private readonly UserManager<User> userManager;

            public ResetPasswordCommandHandler(UserManager<User> userManager)
            {
                this.userManager = userManager;
            }

            public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("Email nicht gefunden");
                }

                var result = await userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

                return Unit.Value;
            }
        }
    }
}
