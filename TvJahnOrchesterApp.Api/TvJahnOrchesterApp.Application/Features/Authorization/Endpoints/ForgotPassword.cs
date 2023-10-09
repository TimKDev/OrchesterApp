using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class ForgotPassword
    {
        public static void MapForgotPasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/forgotPassword", PostRegisterUser);
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
            private readonly IEmailService emailService;

            public ForgotPasswordCommandHandler(IEmailService emailService, UserManager<User> userManager)
            {
                this.emailService = emailService;
                this.userManager = userManager;
            }

            public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("Invalid Request");
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var param = new Dictionary<string, string?>
                {
                    {"token", token },
                    {"email", request.Email }
                };

                var callback = QueryHelpers.AddQueryString(request.ClientUri, param);
                var message = new Message(new string[] { user.Email }, "Reset password token", callback);

                await emailService.SendEmailAsync(message);

                return Unit.Value;
            }
        }
    }
}
