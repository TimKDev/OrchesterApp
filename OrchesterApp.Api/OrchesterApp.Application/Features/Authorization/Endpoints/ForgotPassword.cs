using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors;
using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class ForgotPassword
    {
        public static void MapForgotPasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/forgot-password", PostRegisterUser);
        }

        private static async Task<IResult> PostRegisterUser([FromBody] ForgotPasswordCommand registerUserCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(registerUserCommand);
            return Results.Ok($"Link zum Reseten des Passworts wurde an die Email {registerUserCommand.Email} versendet.");
        }

        private record ForgotPasswordCommand(string Email, string ClientUri) : IRequest<Unit>;

        private class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
        {
            private readonly UserManager<User> userManager;
            private readonly ISendPasswordResetEmailService sendPasswordResetEmailService;

            public ForgotPasswordCommandHandler(UserManager<User> userManager, ISendPasswordResetEmailService sendPasswordResetEmailService)
            {
                this.userManager = userManager;
                this.sendPasswordResetEmailService = sendPasswordResetEmailService;
            }

            public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("Invalid Request");
                }
                if (!user.EmailConfirmed)
                {
                    throw new MailNotVerifiedException(user.Email!);
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                await sendPasswordResetEmailService.SendTo(user.Email!, token, request.ClientUri);

                return Unit.Value;
            }
        }
    }
}
