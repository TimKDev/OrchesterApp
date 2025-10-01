using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface IPortalNotificationBuilder
{
    Task<PortalNotificationContent> BuildAsync(OrchesterApp.Domain.NotificationAggregate.Notification notification,
        CancellationToken cancellationToken);
}