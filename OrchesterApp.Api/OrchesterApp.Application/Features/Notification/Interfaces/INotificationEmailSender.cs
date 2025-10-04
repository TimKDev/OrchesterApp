using OrchesterApp.Domain.UserNotificationAggregate;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface INotificationEmailSender
{
    Task SendEmailsForNotificationAsync(OrchesterApp.Domain.NotificationAggregate.Notification notification,
        IList<UserNotification> userNotifications,
        CancellationToken cancellationToken);
}