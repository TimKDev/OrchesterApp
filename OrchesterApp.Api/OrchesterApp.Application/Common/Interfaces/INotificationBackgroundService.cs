using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces;

public interface INotificationBackgroundService
{
    Task EnqueueNotificationAsync(NotificationId notificationId);
}