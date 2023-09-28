using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class LoginUser
    {
        public static void MapLoginUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Authentication/login", PostLoginUser);
        }

        public static async Task<IResult> PostLoginUser([FromBody] LoginQuery loginQuery, CancellationToken cancellationToken, ISender sender)
        {
            var authResult = await sender.Send(loginQuery);
            return Results.Ok(authResult);
        }

        public record LoginQuery(string Email, string Password, string ClientUri) : IRequest<AuthenticationResult>;

        public class LoginQueryValidator : AbstractValidator<LoginQuery>
        {
            public LoginQueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
        {
            private readonly UserManager<User> userManager;
            private readonly ITokenService tokenService;
            private readonly IEmailService emailService;

            public LoginQueryHandler(UserManager<User> userManager, ITokenService tokenService, IEmailService emailService)
            {
                this.userManager = userManager;
                this.tokenService = tokenService;
                this.emailService = emailService;
            }

            //TTODO Refactor in kleinere Methoden
            public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
            {
                if (await userManager.FindByEmailAsync(request.Email) is not User user)
                {
                    throw new InvalidCredentialsException();
                }
                if (await userManager.IsLockedOutAsync(user))
                {
                    var content = $"Your account is locked out. To reset the password click this link: {request.ClientUri}";
                    var message = new Message(new string[] { request.Email }, "Locked out account information", content);

                    await emailService.SendEmailAsync(message);

                    throw new Exception("Account wurde wegen zu vieler falscher Passworteingaben gesperrt.");
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

                return new AuthenticationResult(user.Id, user.Email, token, refreshToken);
            }
        }

    }
}
