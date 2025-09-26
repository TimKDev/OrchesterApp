using System.Text.Json;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate;

public sealed class ChangeTerminDataNotification : Notification
{
    public int? OldTerminStatus { get; private set; }
    public int? NewTerminStatus { get; private set; }
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

    private ChangeTerminDataNotification(NotificationId id, NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt,
        int? oldTerminStatus, int? newTerminStatus,
        DateTime? oldStartZeit, DateTime? newStartZeit, DateTime? oldEndZeit, DateTime? newEndZeit) : base(id, type,
        category, urgency, terminId, createdAt)
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

        return new ChangeTerminDataNotification(notification.Id, notification.Type, notification.Category,
            notification.Urgency, notification.TerminId, notification.CreatedAt, dataDto.OldTerminStatus,
            dataDto.NewTerminStatus,
            dataDto.OldStartZeit, dataDto.NewStartZeit, dataDto.OldEndZeit, dataDto.NewEndZeit);
    }

    public static ChangeTerminDataNotification New(TerminId terminId, TerminData oldTerminData,
        TerminData newTerminData)
    {
        return new ChangeTerminDataNotification(NotificationId.CreateUnique(), NotificationType.Information,
            NotificationCategory.ChangeTerminData,
            NotificationUrgency.Medium, terminId, DateTime.UtcNow, oldTerminData.TerminStatus,
            newTerminData.TerminStatus, oldTerminData.StartZeit,
            newTerminData.StartZeit, oldTerminData.EndZeit, newTerminData.EndZeit);
    }

    [Serializable]
    private record ChangeTerminDataNotificationDto
    {
        public int? OldTerminStatus { get; init; }
        public int? NewTerminStatus { get; init; }
        public DateTime? OldStartZeit { get; init; }
        public DateTime? NewStartZeit { get; init; }
        public DateTime? OldEndZeit { get; init; }
        public DateTime? NewEndZeit { get; init; }
    }
}