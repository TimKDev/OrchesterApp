using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Domain.UserNotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

public interface INotificationCategoryEmailSender
{
    NotificationCategory NotificationCategory { get; }

    List<Message> CreateMessage(OrchesterApp.Domain.NotificationAggregate.Notification notification,
        IList<UserNotification> userNotifications,
        Dictionary<UserNotification, User?> userNotificationsDict);
}