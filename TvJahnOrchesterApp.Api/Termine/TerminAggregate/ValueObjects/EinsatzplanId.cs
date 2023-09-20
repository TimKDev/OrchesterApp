using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class EinsatzplanId : ValueObject
    {
        public Guid Value { get; private set; }

        private EinsatzplanId() { }

        private EinsatzplanId(Guid value)
        {
            Value = value;
        }

        public static EinsatzplanId CreateUnique()
        {
            return new EinsatzplanId(Guid.NewGuid());
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
