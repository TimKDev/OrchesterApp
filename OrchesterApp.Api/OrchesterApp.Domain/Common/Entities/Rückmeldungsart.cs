using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
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
