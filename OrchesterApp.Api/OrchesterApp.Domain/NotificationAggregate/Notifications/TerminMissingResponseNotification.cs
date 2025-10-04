using System.Text.Json;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Models;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate.Notifications;

public sealed class TerminMissingResponseNotification : Notification
{
    public string TerminName { get; }
    public DateTime TerminDate { get; }

    private TerminMissingResponseNotification(NotificationId id, NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt, string? data,
        string terminName, DateTime terminDate) : base(id, type,
        category, urgency, terminId, createdAt, data)
    {
        TerminName = terminName;
        TerminDate = terminDate;
    }

    public static TerminMissingResponseNotification Create(Notification notification)
    {
        var dataDto = notification.Data is null
            ? new TerminMissingResponseNotificationDto()
            : JsonSerializer.Deserialize<TerminMissingResponseNotificationDto>(notification.Data)
              ?? new TerminMissingResponseNotificationDto();

        return new TerminMissingResponseNotification(notification.Id, notification.Type, notification.Category,
            notification.Urgency, notification.TerminId, notification.CreatedAt, notification.Data,
            dataDto.TerminName, dataDto.TerminDate);
    }

    public static TerminMissingResponseNotification New(TerminId terminId, string terminName, DateTime terminDate)
    {
        var data = JsonSerializer.Serialize(new TerminMissingResponseNotificationDto
        {
            TerminName = terminName,
            TerminDate = terminDate
        });

        return new TerminMissingResponseNotification(NotificationId.CreateUnique(), NotificationType.Failure,
            NotificationCategory.TerminMissingResponse,
            NotificationUrgency.High, terminId, DateTime.UtcNow, data,
            terminName, terminDate);
    }

    public PortalNotificationContent GetPortalNotificationContent()
    {
        return new PortalNotificationContent(
            "Fehlende Rückmeldung",
            $"Die Frist für die Rückmeldung zum Termin '{TerminName}' am {TerminDate:dd.MM.yyyy HH:mm} ist abgelaufen. Bitte melde dich sobald wie möglich zurück.");
    }

    [Serializable]
    private record TerminMissingResponseNotificationDto
    {
        public string TerminName { get; init; } = string.Empty;
        public DateTime TerminDate { get; init; }
    }
}

