using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;
using TvJahnOrchesterApp.Application.Features.Notification.NotificationCategoryEmailSender;
using TvJahnOrchesterApp.Application.Features.Notification.PortalNotificationBuilder;
using TvJahnOrchesterApp.Application.Features.Notification.Services;

namespace TvJahnOrchesterApp.Application.Features.Notification;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationFeature(this IServiceCollection services)
    {
        services.AddScoped<INotificationEmailSender, NotificationEmailSender>();
        services.AddScoped<INotificationCategoryEmailSender, ChangeTerminDataEmailSender>();
        services.AddScoped<INotificationCategoryEmailSender, CustomMessageEmailSender>();
        services.AddScoped<INotificationCategoryEmailSender, TerminReminderEmailSender>();
        services.AddScoped<INotificationCategoryEmailSender, TerminMissingResponseEmailSender>();
        services.AddScoped<IPortalCategoryNotificationBuilder, ChangeTerminDataPortalCategoryBuilder>();
        services.AddScoped<IPortalCategoryNotificationBuilder, CustomMessagePortalCategoryBuilder>();
        services.AddScoped<IPortalCategoryNotificationBuilder, TerminReminderPortalBuilder>();
        services.AddScoped<IPortalCategoryNotificationBuilder, TerminMissingResponsePortalBuilder>();
        services
            .AddScoped<IPortalNotificationBuilder,
                TvJahnOrchesterApp.Application.Features.Notification.Services.PortalNotificationBuilder>();

        return services;
    }
}