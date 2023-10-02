using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public class MitgliedsStatusId: ValueObject, IDropdownId
    {
        public int Value { get; private set; }

        private MitgliedsStatusId() { }

        private MitgliedsStatusId(int id)
        {
            Value = id;
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static MitgliedsStatusId Create(int id)
        {
            return new MitgliedsStatusId(id);
        }
    }
}
