using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Termin.NotificationCategoryEmailSender;

namespace TvJahnOrchesterApp.Application.Features.Termin;

public static class DependencyInjection
{
    public static IServiceCollection AddTerminFeature(this IServiceCollection services)
    {
        services.AddScoped<INotificationCategoryEmailSender, ChangeTerminDataEmailSender>();

        return services;
    }
}