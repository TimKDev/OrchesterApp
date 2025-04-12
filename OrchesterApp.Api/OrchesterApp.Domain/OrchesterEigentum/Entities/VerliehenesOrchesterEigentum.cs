using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.OrchesterEigentum.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace OrchesterApp.Domain.OrchesterEigentum.Entities
{
    public sealed class VerliehenesOrchesterEigentum : Entity<VerliehenesOrchesterEigentumsId>
    {
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; } = null!;
        public int VerliehendeAnzahl { get; private set; }
        public string? Bemerkung { get; private set; }

        private VerliehenesOrchesterEigentum() { }

        private VerliehenesOrchesterEigentum(OrchesterMitgliedsId orchesterMitgliedsId, int verliehendeAnzahl, string? bemerkung)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
            VerliehendeAnzahl = verliehendeAnzahl;
            Bemerkung = bemerkung;
        }

        public static VerliehenesOrchesterEigentum Create(OrchesterMitgliedsId orchesterMitgliedsId, int verliehendeAnzahl, string? bemerkung)
        {
            return new VerliehenesOrchesterEigentum(orchesterMitgliedsId, verliehendeAnzahl, bemerkung);
        }

        public void ErhöheVerleihendeAnzahl(int anzahl)
        {
            VerliehendeAnzahl += anzahl;
        }

        public void VerringereVerleihendeAnzahl(int anzahl)
        {
            if (VerliehendeAnzahl - anzahl < 0)
            {
                throw new ArgumentException("Anzahl kann nicht weiter als auf 0 verringert werden.");
            }
            VerliehendeAnzahl -= anzahl;
        }

        public void UpdateBemerkung(string? neueBemerkung)
        {
            if (neueBemerkung is null) return;
            Bemerkung = neueBemerkung;
        }
    }
}
