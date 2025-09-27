using OrchesterApp.Domain.NotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Services;

public interface INotificationBackgroundService
{
    Task EnqueueNotificationAsync(NotificationId notificationId);
}