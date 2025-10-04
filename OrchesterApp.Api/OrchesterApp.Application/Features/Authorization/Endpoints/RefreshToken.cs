using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors;
using OrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class RefreshToken
    {
        public static void MapRefreshTokenEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/refresh", PostRefreshToken);
        }

        private static async Task<IResult> PostRefreshToken([FromBody] RefreshTokenQuery refreshTokenQuery,
            CancellationToken cancellationToken, ISender sender)
        {
            var authResult = await sender.Send(refreshTokenQuery);
            return Results.Ok(authResult);
        }

        private record RefreshTokenQuery(string Token, string RefreshToken) : IRequest<AuthenticationResult>;

        private class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, AuthenticationResult>
        {
            private readonly IJwtHandler jwtHandler;
            private readonly UserManager<User> userManager;
            private readonly ITokenService tokenService;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepo;

            public RefreshTokenQueryHandler(IJwtHandler jwtHandler, UserManager<User> userManager,
                ITokenService tokenService, IOrchesterMitgliedRepository orchesterMitgliedRepo)
            {
                this.jwtHandler = jwtHandler;
                this.userManager = userManager;
                this.tokenService = tokenService;
                this.orchesterMitgliedRepo = orchesterMitgliedRepo;
            }

            public async Task<AuthenticationResult> Handle(RefreshTokenQuery request,
                CancellationToken cancellationToken)
            {
                var principal = jwtHandler.GetPrincipalFromExpiredToken(request.Token);
                var username = principal?.Identity?.Name;

                var user = await userManager.FindByEmailAsync(username);

                if (user is null || user.Email is null || !tokenService.IsRefreshTokenValid(user, request.RefreshToken))
                {
                    throw new NewLoginNecessaryException();
                }

                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    throw new MailNotVerifiedException(user.Email);
                }

                var roles = await userManager.GetRolesAsync(user);

                var newToken = await tokenService.GenerateAccessTokenAsync(user);
                var newRefreshToken = tokenService.GenerateRefreshToken();

                await tokenService.AddRefreshTokenToUserInDBAsync(user, newRefreshToken);
                var orchesterMitglied = await orchesterMitgliedRepo.GetByUserIdAsync(user.Id, cancellationToken);
                if (orchesterMitglied is null)
                {
                    throw new Exception("Orchestermitglied wurde nicht gefunden");
                }

                return new AuthenticationResult(user.Id, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}",
                    user.Email, newToken, newRefreshToken, roles.ToArray(),
                    TransformImageService.ConvertByteArrayToBase64(orchesterMitglied.Image));
            }
        }
    }
}