using OrchesterApp.Domain.NotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Services;

public interface INotificationEmailSender
{
    Task SendEmailsForNotificationAsync(Notification notification, IList<UserNotification> userNotifications,
        CancellationToken cancellationToken);
}