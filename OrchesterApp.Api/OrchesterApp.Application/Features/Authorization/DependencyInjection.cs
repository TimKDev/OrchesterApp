using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Application.Features.Authorization.Services;

namespace TvJahnOrchesterApp.Application.Features.Authorization
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthorizationFeature(this IServiceCollection services)
        {
            services.AddScoped<IVerificationEmailService, VerificationEmailService>();
            services.AddScoped<ISendRegistrationEmailService, SendRegistrationEmailService>();
            services.AddScoped<ISendPasswordResetEmailService, SendPasswordResetEmailService>();

            return services;
        }
    }
}
