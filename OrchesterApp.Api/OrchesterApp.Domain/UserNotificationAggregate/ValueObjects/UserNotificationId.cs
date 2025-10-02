using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.UserNotificationAggregate.ValueObjects;

public class UserNotificationId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private UserNotificationId()
    {
    }

    private UserNotificationId(Guid value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static UserNotificationId CreateUnique()
    {
        return new UserNotificationId(Guid.NewGuid());
    }

    public static UserNotificationId Create(Guid id)
    {
        return new UserNotificationId(id);
    }
}