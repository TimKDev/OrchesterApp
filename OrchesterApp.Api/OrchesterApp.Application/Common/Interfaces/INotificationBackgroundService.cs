using OrchesterApp.Domain.NotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces;

public interface INotificationBackgroundService
{
    Task EnqueueNotificationAsync(NotificationId notificationId);
}