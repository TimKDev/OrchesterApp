using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
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
