using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.UserAggregate.ValueObjects
{
    public sealed class UserId : AggregateRootId<Guid>
    {
        public override Guid Value { get; protected set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private UserId() { }
        
        private UserId(Guid userId)
        {
            Value = userId;
        }

        public static UserId CreateUnique() 
        {
            return new UserId(Guid.NewGuid());
        }

        public static UserId Create(Guid id)
        {
            return new UserId(id);
        }
    }
}
