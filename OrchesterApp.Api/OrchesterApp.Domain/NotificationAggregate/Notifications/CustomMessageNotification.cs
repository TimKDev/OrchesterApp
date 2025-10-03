using System.Text.Json;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;

namespace OrchesterApp.Domain.NotificationAggregate.Notifications;

public sealed class CustomMessageNotification : Notification
{
    public string Title { get; }
    public string Message { get; }

    private CustomMessageNotification(NotificationId id, NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt, string? data,
        string title, string message) : base(id, type, category, urgency, terminId, createdAt, data)
    {
        Title = title;
        Message = message;
    }

    public static CustomMessageNotification Create(Notification notification)
    {
        var dataDto = notification.Data is null
            ? new CustomMessageNotificationDto()
            : JsonSerializer.Deserialize<CustomMessageNotificationDto>(notification.Data)
              ?? new CustomMessageNotificationDto();

        return new CustomMessageNotification(notification.Id, notification.Type, notification.Category,
            notification.Urgency, notification.TerminId, notification.CreatedAt, notification.Data,
            dataDto.Title, dataDto.Message);
    }

    public static CustomMessageNotification New(string title, string message)
    {
        var data = JsonSerializer.Serialize(new CustomMessageNotificationDto
        {
            Title = title,
            Message = message
        });

        return new CustomMessageNotification(NotificationId.CreateUnique(), NotificationType.Information,
            NotificationCategory.CustomMessage, NotificationUrgency.Medium, null, DateTime.UtcNow, data,
            title, message);
    }

    public PortalNotificationContent GetPortalNotificationContent()
    {
        return new PortalNotificationContent(Title, Message);
    }

    [Serializable]
    private record CustomMessageNotificationDto
    {
        public string Title { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}


