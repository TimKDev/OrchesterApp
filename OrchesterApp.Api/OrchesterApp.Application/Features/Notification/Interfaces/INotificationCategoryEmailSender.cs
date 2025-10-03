using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Domain.UserNotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

public interface INotificationCategoryEmailSender
{
    NotificationCategory NotificationCategory { get; }

    List<Message> CreateMessage(Notification notification, IList<UserNotification> userNotifications,
        Dictionary<UserNotification, User?> userNotificationsDict);
}