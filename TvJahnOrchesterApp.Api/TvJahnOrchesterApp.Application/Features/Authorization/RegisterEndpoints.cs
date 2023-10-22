using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Authorization.Endpoints;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Authorization
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsAuthorizationFeature(this IEndpointRouteBuilder app)
        {
            app.MapRefreshTokenEndpoint();
            app.MapRegisterUserEndpoint();
            app.MapResendVerificationMailEndpoint();
            app.MapForgotPasswordEndpoint();
            app.MapResetPasswordEndpoint();
            app.MapLoginUserEndpoint();
            app.MapDeleteUserEndpoint();
            app.MapGetUserAdminInfosEndpoint();
            app.MapAddRegistrationKeyEndpoint();
            app.MapRevokeUserLockedOutEndpoint();
            app.MapChangeEmailUserEndpoint();
            app.MapConfirmEmailEndpoint();
        }
    }
}
