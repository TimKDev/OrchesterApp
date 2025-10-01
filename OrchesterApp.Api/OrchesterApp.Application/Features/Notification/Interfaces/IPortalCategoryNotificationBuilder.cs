using OrchesterApp.Domain.NotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface IPortalCategoryNotificationBuilder
{
    NotificationCategory NotificationCategory { get; }

    Task<PortalNotificationContent>
        BuildAsync(OrchesterApp.Domain.NotificationAggregate.Notification notification,
            CancellationToken cancellationToken);
}