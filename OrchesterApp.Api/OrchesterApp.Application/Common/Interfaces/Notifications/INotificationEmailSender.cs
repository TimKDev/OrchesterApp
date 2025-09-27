using OrchesterApp.Domain.NotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

public interface INotificationEmailSender
{
    Task SendEmailsForNotificationAsync(Notification notification, IList<UserNotification> userNotifications,
        CancellationToken cancellationToken);
}