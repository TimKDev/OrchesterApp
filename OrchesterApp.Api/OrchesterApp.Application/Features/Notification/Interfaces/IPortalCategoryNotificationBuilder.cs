using OrchesterApp.Domain.NotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

public interface IPortalCategoryNotificationBuilder
{
    NotificationCategory NotificationCategory { get; }
    PortalNotificationContent Build(Notification notification);
}