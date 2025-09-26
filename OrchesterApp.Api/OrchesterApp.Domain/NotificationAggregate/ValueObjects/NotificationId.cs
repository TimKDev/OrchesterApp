using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.NotificationAggregate;

public sealed class NotificationId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private NotificationId()
    {
    }

    private NotificationId(Guid value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static NotificationId CreateUnique()
    {
        return new NotificationId(Guid.NewGuid());
    }

    public static NotificationId Create(Guid id)
    {
        return new NotificationId(id);
    }
}