using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

namespace TvJahnOrchesterApp.Application.Features.Notification.Endpoints;

public interface IPortalNotificationBuilder
{
    PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification);
}