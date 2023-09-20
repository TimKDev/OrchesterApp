using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.ValueObjects
{
    public sealed class Adresse : ValueObject
    {
        public string Straße { get; private set; } = null!;
        public string Hausnummer { get; private set; } = null!;
        public string Postleitzahl { get; private set; } = null!;
        public string Stadt { get; private set; } = null!;
        public string? Zusatz { get; private set; } 
        public decimal? Latitude { get; private set; }
        public decimal? Longitide { get; private  set; }

        private Adresse() { }

        private Adresse(string straße, string hausnummer, string zip, string stadt, string zusatz, decimal? latitude, decimal? longitide)
        {
            Straße = straße;
            Hausnummer = hausnummer;
            Postleitzahl = zip;
            Stadt = stadt;
            Zusatz = zusatz;
            Latitude = latitude;
            Longitide = longitide;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Straße; yield return Hausnummer; yield return Postleitzahl; yield return Stadt;
        }

        public static Adresse Create(string straße, string hausnummer, string zip, string stadt, string? zusatz = null, decimal? latitude = null, decimal? longitide = null)
        {
            return new Adresse(straße, hausnummer, zip, stadt, zusatz, latitude, longitide);
        }
    }
}
