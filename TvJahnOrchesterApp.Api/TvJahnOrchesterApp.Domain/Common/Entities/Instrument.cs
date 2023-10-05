using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class Instrument: Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;
        public int ArtInstrumentId { get; private set; } 

        private Instrument() { }

        private Instrument(int instrumentId, string value, int artInstrumentId)
        {
            Id = instrumentId;
            Value = value;
            ArtInstrumentId = artInstrumentId;
        }

        public static Instrument Create(int id, string name, int artInstrumentId)
        {
            return new Instrument(id, name, artInstrumentId);
        }
    }
}
