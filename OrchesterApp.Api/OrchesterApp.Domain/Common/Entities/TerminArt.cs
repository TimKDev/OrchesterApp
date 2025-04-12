using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
{
    public sealed class TerminArt : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private TerminArt() { }

        private TerminArt(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static TerminArt Create(int id, string value)
        {
            return new TerminArt(id, value);
        }
    }
}
