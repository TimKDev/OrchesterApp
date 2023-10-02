using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public class InstrumentId: ValueObject, IDropdownId
    {
        public int Value { get; private set; }

        private InstrumentId() { }

        private InstrumentId(int id)
        {
            Value = id;
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static InstrumentId Create(int id)
        {
            return new InstrumentId(id);
        }
    }
}
