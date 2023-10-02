using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class Instrument: Entity<InstrumentId>, IDropdownEntity<InstrumentId>
    {
        public string Value { get; private set; } = null!;
        public ArtInstrumentId ArtInstrumentId { get; private set; } = null!;

        private Instrument() { }

        private Instrument(InstrumentId instrumentId, string value, ArtInstrumentId artInstrumentId)
        {
            Id = instrumentId;
            Value = value;
            ArtInstrumentId = artInstrumentId;
        }

        public static Instrument Create(int id, string name, ArtInstrumentId artInstrumentId)
        {
            return new Instrument(InstrumentId.Create(id), name, artInstrumentId);
        }
    }
}
