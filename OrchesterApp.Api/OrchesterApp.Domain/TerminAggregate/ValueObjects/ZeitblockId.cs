using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class ZeitblockId : ValueObject
    {
        public Guid Value { get; private set; }

        private ZeitblockId()
        {
        }

        private ZeitblockId(Guid value)
        {
            Value = value;
        }

        public static ZeitblockId CreateUnique()
        {
            return new ZeitblockId(Guid.NewGuid());
        }

        public static ZeitblockId Create(Guid zeitBlockId)
        {
            return new ZeitblockId(zeitBlockId);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}