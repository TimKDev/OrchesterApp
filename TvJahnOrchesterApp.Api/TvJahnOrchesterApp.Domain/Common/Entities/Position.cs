using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public class Position : Entity<PositionId>, IDropdownEntity<PositionId>
    {
        public string Value { get; private set; } = null!;

        private Position() { }

        private Position(PositionId id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Position Create(int id, string value)
        {
            return new Position(PositionId.Create(id), value);
        }
    }
}
