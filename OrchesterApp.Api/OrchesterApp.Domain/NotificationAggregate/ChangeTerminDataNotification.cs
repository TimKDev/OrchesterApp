using System.Text.Json;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate;

public sealed class ChangeTerminDataNotification : Notification
{
    public TerminStatus? OldTerminStatus { get; private set; }
    public TerminStatus? NewTerminStatus { get; private set; }
    public DateTime? OldStartZeit { get; private set; }
    public DateTime? NewStartZeit { get; private set; }
    public DateTime? OldEndZeit { get; private set; }
    public DateTime? NewEndZeit { get; private set; }

    public override string Data => JsonSerializer.Serialize(new ChangeTerminDataNotificationDto()
    {
        OldTerminStatus = OldTerminStatus,
        NewTerminStatus = NewTerminStatus,
        OldStartZeit = OldStartZeit,
        NewStartZeit = NewStartZeit,
        OldEndZeit = OldEndZeit,
        NewEndZeit = NewEndZeit,
    });

    private ChangeTerminDataNotification(NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt,
        HashSet<UserNotificationId> userNotificationIds, TerminStatus? oldTerminStatus,
        TerminStatus? newTerminStatus,
        DateTime? oldStartZeit, DateTime? newStartZeit, DateTime? oldEndZeit, DateTime? newEndZeit) : base(type,
        category, urgency, terminId, createdAt, userNotificationIds)
    {
        OldTerminStatus = oldTerminStatus;
        NewTerminStatus = newTerminStatus;
        OldStartZeit = oldStartZeit;
        NewStartZeit = newStartZeit;
        OldEndZeit = oldEndZeit;
        NewEndZeit = newEndZeit;
    }

    public static ChangeTerminDataNotification Create(Notification notification)
    {
        var dataDto = notification.Data is null
            ? new ChangeTerminDataNotificationDto()
            : JsonSerializer.Deserialize<ChangeTerminDataNotificationDto>(notification.Data)
              ?? new ChangeTerminDataNotificationDto();

        return new ChangeTerminDataNotification(notification.Type, notification.Category,
            notification.Urgency, notification.TerminId, notification.CreatedAt,
            notification.UserNotifications.ToHashSet(), dataDto.OldTerminStatus, dataDto.NewTerminStatus,
            dataDto.OldStartZeit, dataDto.NewStartZeit, dataDto.OldEndZeit, dataDto.NewEndZeit);
    }

    public static ChangeTerminDataNotification New(TerminId terminId)
    {
        return new ChangeTerminDataNotification(NotificationType.Information, NotificationCategory.ChangeTerminData,
            NotificationUrgency.Medium, terminId, DateTime.UtcNow,
            [], null, null, null, null, null, null);
    }

    public ChangeTerminDataNotification NotifyAboutTerminStatusChange(TerminStatus oldStatus,
        TerminStatus newStatus)
    {
        OldTerminStatus = oldStatus;
        NewTerminStatus = newStatus;

        return this;
    }

    public ChangeTerminDataNotification NotifyAboutStartTimeChange(DateTime oldStartZeit, DateTime newStartZeit)
    {
        OldStartZeit = oldStartZeit;
        NewStartZeit = newStartZeit;

        return this;
    }

    public ChangeTerminDataNotification NotifyAboutEndTimeChange(DateTime oldEndZeit, DateTime newEndZeit)
    {
        OldEndZeit = oldEndZeit;
        NewEndZeit = newEndZeit;

        return this;
    }

    [Serializable]
    private record ChangeTerminDataNotificationDto
    {
        public TerminStatus? OldTerminStatus { get; init; }
        public TerminStatus? NewTerminStatus { get; init; }
        public DateTime? OldStartZeit { get; init; }
        public DateTime? NewStartZeit { get; init; }
        public DateTime? OldEndZeit { get; init; }
        public DateTime? NewEndZeit { get; init; }
    }
}