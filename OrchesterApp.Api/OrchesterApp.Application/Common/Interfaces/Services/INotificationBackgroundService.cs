using OrchesterApp.Domain.NotificationAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services;

public interface INotificationBackgroundService
{
    Task EnqueueNotificationAsync(NotificationId notificationId);
}