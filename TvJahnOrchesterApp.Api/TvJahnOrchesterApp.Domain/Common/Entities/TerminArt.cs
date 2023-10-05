using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
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
