using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects
{
    public sealed class OrchesterMitgliedPositionsMappingId : ValueObject
    {
        public int Value { get; private set; }

        private OrchesterMitgliedPositionsMappingId() { }

        private OrchesterMitgliedPositionsMappingId(int value) 
        { 
            Value = value; 
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        public static OrchesterMitgliedPositionsMappingId Create(int value)
        {
            return new OrchesterMitgliedPositionsMappingId(value);
        }
    }
}
