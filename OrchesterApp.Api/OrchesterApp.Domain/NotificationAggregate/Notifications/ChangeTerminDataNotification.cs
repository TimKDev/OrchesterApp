using System.Text.Json;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate;

public sealed class ChangeTerminDataNotification : Notification
{
    public int? OldTerminStatus { get; }
    public int? NewTerminStatus { get; }
    public DateTime? OldStartZeit { get; }
    public DateTime? NewStartZeit { get; }
    public DateTime? OldEndZeit { get; }
    public DateTime? NewEndZeit { get; }

    private ChangeTerminDataNotification(NotificationId id, NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt, string? data,
        int? oldTerminStatus, int? newTerminStatus,
        DateTime? oldStartZeit, DateTime? newStartZeit, DateTime? oldEndZeit, DateTime? newEndZeit) : base(id, type,
        category, urgency, terminId, createdAt, data)
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
            notification.Urgency, notification.TerminId, notification.CreatedAt, notification.Data,
            dataDto.OldTerminStatus, dataDto.NewTerminStatus, dataDto.OldStartZeit, dataDto.NewStartZeit,
            dataDto.OldEndZeit, dataDto.NewEndZeit);
    }

    public static ChangeTerminDataNotification New(TerminId terminId, TerminData oldTerminData,
        TerminData newTerminData)
    {
        var doesStartTimeChange = oldTerminData.StartZeit != newTerminData.StartZeit;
        var doesEndTimeChange = oldTerminData.EndZeit != newTerminData.EndZeit;
        var doesTerminStatusChange = oldTerminData.TerminStatus != newTerminData.TerminStatus;

        var oldStatusValue = doesTerminStatusChange ? oldTerminData.TerminStatus : null;
        var newStatusValue = doesTerminStatusChange ? newTerminData.TerminStatus : null;
        DateTime? oldStartTimeValue = doesStartTimeChange ? oldTerminData.StartZeit : null;
        DateTime? newStartTimeValue = doesStartTimeChange ? newTerminData.StartZeit : null;
        DateTime? oldEndValue = doesEndTimeChange ? oldTerminData.EndZeit : null;
        DateTime? newEndValue = doesEndTimeChange ? newTerminData.EndZeit : null;

        var data = JsonSerializer.Serialize(new ChangeTerminDataNotificationDto()
        {
            OldTerminStatus = oldStatusValue,
            NewTerminStatus = newStatusValue,
            OldStartZeit = oldStartTimeValue,
            NewStartZeit = newStartTimeValue,
            OldEndZeit = oldEndValue,
            NewEndZeit = newEndValue,
        });

        return new ChangeTerminDataNotification(NotificationId.CreateUnique(), NotificationType.Information,
            NotificationCategory.ChangeTerminData,
            NotificationUrgency.Medium, terminId, DateTime.UtcNow, data,
            oldStatusValue,
            newStatusValue,
            oldStartTimeValue,
            newStartTimeValue,
            oldEndValue, newEndValue);
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