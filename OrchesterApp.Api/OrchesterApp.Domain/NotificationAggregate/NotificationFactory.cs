namespace OrchesterApp.Domain.NotificationAggregate;

public static class NotificationFactory
{
    public static TNotification Create<TNotification>(Notification notification) where TNotification : Notification
    {
        var result = notification.Category switch
        {
            NotificationCategory.ChangeTerminData => ChangeTerminDataNotification.Create(
                notification),
            _ => throw new ArgumentOutOfRangeException()
        };

        return result as TNotification ??
               throw new Exception("Provided notification does not match expected notification type.");
    }
}