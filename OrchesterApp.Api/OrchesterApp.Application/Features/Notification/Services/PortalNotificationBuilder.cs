using OrchesterApp.Domain.NotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Features.Notification.Endpoints;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Notification.Services;

public class PortalNotificationBuilder : IPortalNotificationBuilder
{
    private readonly Dictionary<NotificationCategory, IPortalCategoryNotificationBuilder> _notificationBuilders;

    public PortalNotificationBuilder(IEnumerable<IPortalCategoryNotificationBuilder> notificationBuilders)
    {
        _notificationBuilders = notificationBuilders.ToDictionary(n => n.NotificationCategory, n => n);
    }

    public async Task<PortalNotificationContent> BuildAsync(
        OrchesterApp.Domain.NotificationAggregate.Notification notification,
        CancellationToken cancellationToken = default)
    {
        if (!_notificationBuilders.TryGetValue(notification.Category, out var notificationBuilder))
        {
            throw new Exception($"No notification builder found for notification category {notification.Category}");
        }

        return await notificationBuilder.BuildAsync(NotificationFactory.Create(notification), cancellationToken);
    }
}