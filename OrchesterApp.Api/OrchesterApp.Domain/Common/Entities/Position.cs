using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
{
    public sealed class Position : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private Position() { }

        private Position(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Position Create(int id, string value)
        {
            return new Position(id, value);
        }
    }
}
