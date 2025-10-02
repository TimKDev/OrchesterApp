using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Models;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Notification.Services;

public class PortalNotificationBuilder : IPortalNotificationBuilder
{
    private readonly Dictionary<NotificationCategory, IPortalCategoryNotificationBuilder> _notificationBuilders;

    public PortalNotificationBuilder(IEnumerable<IPortalCategoryNotificationBuilder> notificationBuilders)
    {
        _notificationBuilders = notificationBuilders.ToDictionary(n => n.NotificationCategory, n => n);
    }

    public PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification)
    {
        if (!_notificationBuilders.TryGetValue(notification.Category, out var notificationBuilder))
        {
            throw new Exception($"No notification builder found for notification category {notification.Category}");
        }

        return notificationBuilder.Build(NotificationFactory.Create(notification));
    }
}