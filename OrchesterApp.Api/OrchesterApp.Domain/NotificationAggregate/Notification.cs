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
        public virtual string? Data { get; private set; }

        protected Notification(NotificationType type, NotificationCategory category, NotificationUrgency urgency,
            TerminId? terminId)
        {
            Type = type;
            Category = category;
            Urgency = urgency;
            TerminId = terminId;
        }

        private Notification()
        {
        }
    }
}