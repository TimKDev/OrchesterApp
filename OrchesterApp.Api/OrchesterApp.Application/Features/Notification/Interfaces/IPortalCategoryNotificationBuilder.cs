using OrchesterApp.Domain.NotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface IPortalCategoryNotificationBuilder
{
    NotificationCategory NotificationCategory { get; }
    PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification);
}