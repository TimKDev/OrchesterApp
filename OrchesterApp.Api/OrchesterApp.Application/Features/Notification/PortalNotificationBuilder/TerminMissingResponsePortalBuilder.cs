using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Models;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Notification.PortalNotificationBuilder;

public class TerminMissingResponsePortalBuilder : IPortalCategoryNotificationBuilder
{
    public NotificationCategory NotificationCategory => NotificationCategory.TerminMissingResponse;

    public PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification)
    {
        if (notification is not TerminMissingResponseNotification missingResponseNotification)
        {
            throw new ApplicationException("Invalid notification type");
        }

        return missingResponseNotification.GetPortalNotificationContent();
    }
}

