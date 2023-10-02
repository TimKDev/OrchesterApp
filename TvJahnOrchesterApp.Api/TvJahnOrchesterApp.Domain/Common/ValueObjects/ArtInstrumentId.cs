using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public class ArtInstrumentId : ValueObject, IDropdownId
    {
        public int Value { get; private set; }

        private ArtInstrumentId() { }

        private ArtInstrumentId(int id)
        {
            Value = id;
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static ArtInstrumentId Create(int id)
        {
            return new ArtInstrumentId(id);
        }
    }
}
