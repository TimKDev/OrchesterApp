using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.Notification.Models;

public record NotificationDto(
    UserNotificationId Id,
    string Title,
    string Message,
    bool IsRead,
    NotificationType Type,
    NotificationUrgency Urgency,
    TerminId? TerminId,
    DateTime CreatedAt
);