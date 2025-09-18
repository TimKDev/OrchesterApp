using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.NotificationAggregate
{
    public sealed class Notification : AggregateRoot<NotificationId, Guid>
    {
    }
}