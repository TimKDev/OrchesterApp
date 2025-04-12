using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class TerminId : AggregateRootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private TerminId() { }

        private TerminId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static TerminId CreateUnique()
        {
            return new TerminId(Guid.NewGuid());
        }

        public static TerminId Create(Guid id)
        {
            return new TerminId(id);
        }
    }
}
