using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate
{
    public class Notification : AggregateRoot<NotificationId, Guid>
    {
        private readonly HashSet<UserNotificationId> _userNotificationIds;
        public NotificationType Type { get; private set; }
        public NotificationCategory Category { get; private set; }
        public NotificationUrgency Urgency { get; private set; }
        public TerminId? TerminId { get; private set; }
        public virtual string? Data { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public IReadOnlySet<UserNotificationId> UserNotifications => _userNotificationIds;

        protected Notification(NotificationType type, NotificationCategory category, NotificationUrgency urgency,
            TerminId? terminId, DateTime createdAt, HashSet<UserNotificationId> userNotificationIds)
        {
            Type = type;
            Category = category;
            Urgency = urgency;
            TerminId = terminId;
            CreatedAt = createdAt;
            _userNotificationIds = userNotificationIds;
        }

        private Notification()
        {
        }
    }
}