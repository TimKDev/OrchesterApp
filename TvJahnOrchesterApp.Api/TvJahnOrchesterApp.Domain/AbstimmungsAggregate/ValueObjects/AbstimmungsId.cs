using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects
{
    public sealed class AbstimmungsId : AggregateRootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private AbstimmungsId() { }

        private AbstimmungsId(Guid value)
        {
            Value = value;
        }

        public static AbstimmungsId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static AbstimmungsId Create(Guid id)
        {
            return new AbstimmungsId(id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
