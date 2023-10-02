using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public class PositionId: ValueObject, IDropdownId
    {
        public int Value { get; private set; }

        private PositionId() { }

        private PositionId(int id)
        {
            Value = id;
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static PositionId Create(int id)
        {
            return new PositionId(id);
        }
    }
}
