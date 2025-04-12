using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
{
    public sealed class ArtInstrument : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private ArtInstrument() { }

        private ArtInstrument(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static ArtInstrument Create(int id, string value)
        {
            return new ArtInstrument(id, value);
        }
    }
}
