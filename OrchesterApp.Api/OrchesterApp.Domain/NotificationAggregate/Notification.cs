using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate
{
    public class Notification : AggregateRoot<NotificationId, Guid>
    {
        public NotificationType Type { get; private set; }
        public NotificationCategory Category { get; private set; }
        public NotificationUrgency Urgency { get; private set; }
        public TerminId? TerminId { get; private set; }
        public string? Data { get; }

        public DateTime CreatedAt { get; private set; }

        protected Notification(NotificationId id, NotificationType type, NotificationCategory category,
            NotificationUrgency urgency,
            TerminId? terminId, DateTime createdAt, string? data)
        {
            Id = id;
            Type = type;
            Category = category;
            Urgency = urgency;
            TerminId = terminId;
            CreatedAt = createdAt;
            Data = data;
        }
    }
}