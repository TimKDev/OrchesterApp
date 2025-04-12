using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.AbstimmungsAggregate.ValueObjects
{
    public sealed class UserAbstimmungsId : ValueObject
    {
        public Guid Value { get; private set; }

        private UserAbstimmungsId() { }

        private UserAbstimmungsId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value; 
        }

        public static UserAbstimmungsId CreateUnique()
        {
            return new UserAbstimmungsId(Guid.NewGuid());
        }
    }
}
