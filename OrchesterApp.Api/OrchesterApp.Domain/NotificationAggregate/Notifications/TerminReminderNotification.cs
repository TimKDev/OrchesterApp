using System.Text.Json;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Models;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate.Notifications;

public sealed class TerminReminderNotification : Notification
{
    public string TerminName { get; }
    public DateTime TerminDate { get; }
    public DateTime TerminDeadline { get; }

    private TerminReminderNotification(NotificationId id, NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt, string? data,
        string terminName, DateTime terminDate, DateTime terminDeadline) : base(id, type,
        category, urgency, terminId, createdAt, data)
    {
        TerminName = terminName;
        TerminDate = terminDate;
        TerminDeadline = terminDeadline;
    }

    public static TerminReminderNotification Create(Notification notification)
    {
        var dataDto = notification.Data is null
            ? new TerminReminderNotificationDto()
            : JsonSerializer.Deserialize<TerminReminderNotificationDto>(notification.Data)
              ?? new TerminReminderNotificationDto();

        return new TerminReminderNotification(notification.Id, notification.Type, notification.Category,
            notification.Urgency, notification.TerminId, notification.CreatedAt, notification.Data,
            dataDto.TerminName, dataDto.TerminDate, dataDto.TerminDeadline);
    }

    public static TerminReminderNotification New(TerminId terminId, string terminName, DateTime terminDate, DateTime terminDeadline)
    {
        var data = JsonSerializer.Serialize(new TerminReminderNotificationDto
        {
            TerminName = terminName,
            TerminDate = terminDate,
            TerminDeadline = terminDeadline
        });

        return new TerminReminderNotification(NotificationId.CreateUnique(), NotificationType.Warning,
            NotificationCategory.TerminReminderBeforeDeadline,
            NotificationUrgency.High, terminId, DateTime.UtcNow, data,
            terminName, terminDate, terminDeadline);
    }

    public PortalNotificationContent GetPortalNotificationContent()
    {
        return new PortalNotificationContent(
            "Erinnerung: Rückmeldung ausstehend",
            $"Bitte melde dich bis {TerminDeadline:dd.MM.yyyy HH:mm} für den Termin '{TerminName}' am {TerminDate:dd.MM.yyyy HH:mm} zurück.");
    }

    [Serializable]
    private record TerminReminderNotificationDto
    {
        public string TerminName { get; init; } = string.Empty;
        public DateTime TerminDate { get; init; }
        public DateTime TerminDeadline { get; init; }
    }
}

