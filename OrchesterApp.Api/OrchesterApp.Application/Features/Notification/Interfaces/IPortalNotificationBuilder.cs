using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface IPortalNotificationBuilder
{
    PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification);
}