using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate;

public class UserNotification : AggregateRoot<UserNotificationId, Guid>
{
    public UserId UserId { get; private set; }
    public NotificationId NotificationId { get; private set; }
    public SendStatus SendStatus { get; private set; }
    public DateTime? SendAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public NotificationSink NotificationSink { get; private set; }
    public bool IsRead { get; private set; } = false;

    private UserNotification(UserNotificationId id, UserId userId, NotificationId notificationId,
        SendStatus sendStatus, DateTime createdAt, DateTime? sendAt, NotificationSink notificationSink) : base(id)
    {
        UserId = userId;
        NotificationId = notificationId;
        SendStatus = sendStatus;
        SendAt = sendAt;
        CreatedAt = createdAt;
        NotificationSink = notificationSink;
    }

    private UserNotification()
    {
    }

    public static UserNotification Create(UserId userId, NotificationId notificationId,
        NotificationSink notificationSink)
    {
        return new UserNotification(UserNotificationId.CreateUnique(), userId, notificationId, SendStatus.Pending,
            DateTime.UtcNow,
            null, notificationSink);
    }

    public void SendedSuccessfully()
    {
        SendStatus = SendStatus.Success;
        SendAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}