using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors;
using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class LoginUser
    {
        public static void MapLoginUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/login", PostLoginUser);
        }

        private static async Task<IResult> PostLoginUser([FromBody] LoginQuery loginQuery, ISender sender,
            CancellationToken cancellationToken)
        {
            var authResult = await sender.Send(loginQuery);
            return Results.Ok(authResult);
        }

        private record LoginQuery(string Email, string Password, string ClientUri) : IRequest<AuthenticationResult>;

        private class LoginQueryValidator : AbstractValidator<LoginQuery>
        {
            public LoginQueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        private class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
        {
            private readonly UserManager<User> userManager;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepo;
            private readonly ITokenService tokenService;
            private readonly IEmailService emailService;
            private readonly IUnitOfWork unitOfWork;

            public LoginQueryHandler(UserManager<User> userManager, ITokenService tokenService,
                IEmailService emailService, IOrchesterMitgliedRepository orchesterMitgliedRepo, IUnitOfWork unitOfWork)
            {
                this.userManager = userManager;
                this.tokenService = tokenService;
                this.emailService = emailService;
                this.orchesterMitgliedRepo = orchesterMitgliedRepo;
                this.unitOfWork = unitOfWork;
            }

            public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
            {
                if (await userManager.FindByEmailAsync(request.Email) is not User user)
                {
                    throw new InvalidCredentialsException();
                }

                if (await userManager.IsLockedOutAsync(user))
                {
                    var content =
                        $"Your account is locked out. To reset the password click this link: {request.ClientUri}";
                    var message = new Message(new string[] { request.Email }, "Locked out account information",
                        content);

                    await emailService.SendEmailAsync(message);

                    throw new AccountLockedException();
                }

                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    throw new MailNotVerifiedException(request.Email);
                }

                if (!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    await userManager.AccessFailedAsync(user);
                    throw new InvalidCredentialsException();
                }

                var token = await tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = tokenService.GenerateRefreshToken();

                await tokenService.AddRefreshTokenToUserInDBAsync(user, refreshToken);

                await userManager.ResetAccessFailedCountAsync(user);

                var roles = await userManager.GetRolesAsync(user);

                var orchesterMitglied = await orchesterMitgliedRepo.GetByUserIdAsync(user.Id, cancellationToken);

                orchesterMitglied!.UserLogin();
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return new AuthenticationResult(user.Id, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}",
                    user.Email, token, refreshToken, roles.ToArray(),
                    TransformImageService.ConvertByteArrayToBase64(orchesterMitglied.Image));
            }
        }
    }
}