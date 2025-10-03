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
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}