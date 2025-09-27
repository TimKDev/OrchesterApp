using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Common.Interfaces;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Services;

namespace TvJahnOrchesterApp.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddScoped<INotificationEmailSender, NotificationEmailSender>();
        services.AddScoped<INotifyService, NotifyService>();

        services.AddSingleton<NotificationBackgroundService>();

        services.AddSingleton<INotificationBackgroundService>(provider =>
            provider.GetRequiredService<NotificationBackgroundService>());

        services.AddHostedService<NotificationBackgroundService>(provider =>
            provider.GetRequiredService<NotificationBackgroundService>());

        return services;
    }
}