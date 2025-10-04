using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.Notification.Models;

public record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    bool IsRead,
    NotificationType Type,
    NotificationUrgency Urgency,
    Guid? TerminId,
    DateTime CreatedAt
);