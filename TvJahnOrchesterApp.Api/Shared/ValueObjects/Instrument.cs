using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public sealed class Instrument : ValueObject
    {
        public string Name { get; private set; }
        public ArtInstrument ArtInstrument { get; private set; }

        private Instrument() { }

        private Instrument(string name, ArtInstrument artInstrument)
        {
            Name = name;
            ArtInstrument = artInstrument;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ArtInstrument;
        }

        public static Instrument Create(string name, ArtInstrument artInstrument)
        {
            return new Instrument(name, artInstrument);
        }
    }
}
