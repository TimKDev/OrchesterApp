using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class Rückmeldungsart : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private Rückmeldungsart() { }

        private Rückmeldungsart(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Rückmeldungsart Create(int id, string value)
        {
            return new Rückmeldungsart(id, value);
        }
    }
}
