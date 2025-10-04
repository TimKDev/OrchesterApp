using OrchesterApp.Domain.NotificationAggregate.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface IPortalNotificationBuilder
{
    PortalNotificationContent Build(OrchesterApp.Domain.NotificationAggregate.Notification notification);
}