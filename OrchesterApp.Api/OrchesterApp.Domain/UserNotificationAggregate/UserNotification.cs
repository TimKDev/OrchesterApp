using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate;

public class UserNotification : AggregateRoot<UserNotificationId, Guid>
{
    public UserId UserId { get; private set; }
    public NotificationId NotificationId { get; private set; }
    public SendStatus SendStatus { get; private set; }
    public DateTime? SendAt { get; private set; }
    public NotificationSink NotificationSink { get; private set; }

    private UserNotification(UserNotificationId id, UserId userId, NotificationId notificationId,
        SendStatus sendStatus, DateTime? sendAt, NotificationSink notificationSink) : base(id)
    {
        UserId = userId;
        NotificationId = notificationId;
        SendStatus = sendStatus;
        SendAt = sendAt;
        NotificationSink = notificationSink;
    }

    private UserNotification()
    {
    }

    public static UserNotification Create(UserId userId, NotificationId notificationId,
        NotificationSink notificationSink)
    {
        return new UserNotification(UserNotificationId.CreateUnique(), userId, notificationId, SendStatus.Pending,
            null, notificationSink);
    }

    public void SendedSuccessfully()
    {
        SendStatus = SendStatus.Success;
        SendAt = DateTime.UtcNow;
    }
}