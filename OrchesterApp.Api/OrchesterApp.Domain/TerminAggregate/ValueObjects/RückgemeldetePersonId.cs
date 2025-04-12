using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class RückgemeldetePersonId : ValueObject
    {
        public Guid Value { get; private set; }

        private RückgemeldetePersonId() { }

        private RückgemeldetePersonId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static RückgemeldetePersonId CreateUnique()
        {
            return new RückgemeldetePersonId(Guid.NewGuid());
        }

        public static RückgemeldetePersonId Create(Guid id)
        {
            return new RückgemeldetePersonId(id);
        }
    }
}
