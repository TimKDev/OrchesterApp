using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public class NotenstimmeId : ValueObject, IDropdownId
    {
        public int Value { get; private set; }

        private NotenstimmeId() { }

        private NotenstimmeId(int id)
        {
            Value = id;
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static NotenstimmeId Create(int id)
        {
            return new NotenstimmeId(id);
        }
    }
}
