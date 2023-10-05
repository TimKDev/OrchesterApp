using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class Noten : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private Noten() { }

        private Noten(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Noten Create(int id, string value)
        {
            return new Noten(id, value);
        }
    }
}
