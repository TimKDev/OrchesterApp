using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserNotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

public interface INotificationEmailSender
{
    Task SendEmailsForNotificationAsync(Notification notification, IList<UserNotification> userNotifications,
        CancellationToken cancellationToken);
}