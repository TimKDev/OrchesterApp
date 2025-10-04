using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Models;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Notification.PortalNotificationBuilder;

public class ChangeTerminDataPortalCategoryBuilder : IPortalCategoryNotificationBuilder
{
    public NotificationCategory NotificationCategory => NotificationCategory.ChangeTerminData;

    public PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification)
    {
        if (notification is not ChangeTerminDataNotification changeTerminDataNotification)
        {
            throw new ApplicationException("Invalid notification type");
        }

        return changeTerminDataNotification.GetPortalNotificationContent();
    }
}