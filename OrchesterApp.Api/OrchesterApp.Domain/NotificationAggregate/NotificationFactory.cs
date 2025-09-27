namespace OrchesterApp.Domain.NotificationAggregate;

public static class NotificationFactory
{
    public static Notification Create(Notification notification)
    {
        var result = notification.Category switch
        {
            NotificationCategory.ChangeTerminData => ChangeTerminDataNotification.Create(
                notification),
            _ => throw new ArgumentOutOfRangeException()
        };

        return result;
    }
}