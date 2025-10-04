using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Notifications;

namespace OrchesterApp.Domain.NotificationAggregate;

public static class NotificationFactory
{
    public static Notification Create(Notification notification)
    {
        return notification.Category switch
        {
            NotificationCategory.ChangeTerminData => ChangeTerminDataNotification.Create(
                notification),
            NotificationCategory.CustomMessage => CustomMessageNotification.Create(
                notification),
            NotificationCategory.TerminReminderBeforeDeadline => TerminReminderNotification.Create(notification),
            NotificationCategory.TerminMissingResponse => TerminMissingResponseNotification.Create(notification),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}