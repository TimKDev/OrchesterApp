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
    public static class ChangeUserEmail
    {
        public static void MapChangeEmailUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/authentication/change-user-email", PutChangeUserEmail);
        }

        private static async Task<IResult> PutChangeUserEmail([FromBody] ChangeUserEmailCommand changeUserEmailCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(changeUserEmailCommand);
            return Results.Ok("User Email wurde erfolgreich geändert");
        }

        private record ChangeUserEmailCommand(string OldEmail, string Password, string NewEmail, string ClientUri) : IRequest<Unit>;

        private class ChangeUserEmailCommandHandler : IRequestHandler<ChangeUserEmailCommand, Unit>
        {
            private readonly UserManager<User> userManager;
            private readonly IUnitOfWork unitOfWork;
            private readonly IVerificationEmailService verificationEmailService;


            public ChangeUserEmailCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork, IVerificationEmailService verificationEmailService)
            {
                this.userManager = userManager;
                this.unitOfWork = unitOfWork;
                this.verificationEmailService = verificationEmailService;
            }

            public async Task<Unit> Handle(ChangeUserEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.OldEmail);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if(!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    throw new Exception("Unauthorized");
                }
                var userWithNewMail = await userManager.FindByEmailAsync(request.NewEmail);
                if(userWithNewMail is not null)
                {
                    throw new Exception("Email adresse is already taken!");
                }
                var validUserEmailToken = await userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
                await userManager.ChangeEmailAsync(user, request.NewEmail, validUserEmailToken);
                user.EmailConfirmed = false;
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await verificationEmailService.SendTo(user, request.ClientUri);


                return Unit.Value;
            }
        }
    }
}
