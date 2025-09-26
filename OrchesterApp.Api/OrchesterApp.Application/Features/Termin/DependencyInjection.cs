using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Common.Services;

namespace TvJahnOrchesterApp.Application.Features.Termin;

public static class DependencyInjection
{
    public static IServiceCollection AddTerminFeature(this IServiceCollection services)
    {
        services.AddScoped<INotificationCategoryEmailSender, ChangeTerminDataEmailSender>();

        return services;
    }
}