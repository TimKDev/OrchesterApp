using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class ArtInstrument : Entity<ArtInstrumentId>, IDropdownEntity<ArtInstrumentId>
    {
        public string Value { get; private set; } = null!;

        private ArtInstrument() { }

        private ArtInstrument(int id, string value)
        {
            Id = ArtInstrumentId.Create(id);
            Value = value;
        }

        public static ArtInstrument Create(int id, string value)
        {
            return new ArtInstrument(id, value);
        }
    }
}
