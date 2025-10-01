using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;
using TvJahnOrchesterApp.Application.Features.Notification.NotificationCategoryEmailSender;
using TvJahnOrchesterApp.Application.Features.Notification.PortalNotificationBuilder;

namespace TvJahnOrchesterApp.Application.Features.Notification;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationFeature(this IServiceCollection services)
    {
        services.AddScoped<INotificationEmailSender, NotificationEmailSender>();
        services.AddScoped<INotificationCategoryEmailSender, ChangeTerminDataEmailSender>();
        services.AddScoped<IPortalCategoryNotificationBuilder, ChangeTerminDataPortalCategoryBuilder>();
        services
            .AddScoped<IPortalNotificationBuilder,
                TvJahnOrchesterApp.Application.Features.Notification.Services.PortalNotificationBuilder>();

        return services;
    }
}