using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class ResendVerificationMail
    {
        public static void MapResendVerificationMailEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/resendMail", PostResendVerificationMail);
        }

        private static async Task<IResult> PostResendVerificationMail([FromBody] ResendVerficationMailCommand resendVerficationMailCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(resendVerficationMailCommand);
            return Results.Ok("Mail wurde erneut versendet.");
        }

        private record ResendVerficationMailCommand(string Email, string ClientUri) : IRequest<Unit>;

        private class ResendVerficationMailCommandHandler : IRequestHandler<ResendVerficationMailCommand, Unit>
        {
            private readonly UserManager<User> userManager;
            private readonly IVerificationEmailService verificationEmailService;

            public ResendVerficationMailCommandHandler(UserManager<User> userManager, IVerificationEmailService verificationEmailService)
            {
                this.userManager = userManager;
                this.verificationEmailService = verificationEmailService;
            }

            public async Task<Unit> Handle(ResendVerficationMailCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if(user is null)
                {
                    throw new Exception("Email nicht gefunden");
                }
                await verificationEmailService.SendTo(user, request.ClientUri);
                return Unit.Value;
            }
        }

    }
}
