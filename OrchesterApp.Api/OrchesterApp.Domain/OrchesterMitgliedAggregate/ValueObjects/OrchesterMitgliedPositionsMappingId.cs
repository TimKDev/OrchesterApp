using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects
{
    public sealed class OrchesterMitgliedPositionsMappingId : ValueObject
    {
        public Guid Value { get; private set; }

        private OrchesterMitgliedPositionsMappingId() { }

        private OrchesterMitgliedPositionsMappingId(Guid value) 
        { 
            Value = value; 
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static OrchesterMitgliedPositionsMappingId Create(Guid value)
        {
            return new OrchesterMitgliedPositionsMappingId(value);
        }

        public static OrchesterMitgliedPositionsMappingId CreateUnique()
        {
            return new OrchesterMitgliedPositionsMappingId(Guid.NewGuid());
        }
    }
}
