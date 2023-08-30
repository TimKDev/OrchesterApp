using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterEigentum.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Enums;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate
{
    public sealed class OrchesterMitglied: AggregateRoot<OrchesterMitgliedsId, Guid>
    {
        private readonly List<TerminRückmeldung> _terminRückmeldungen = new();
        private readonly List<Position> _positions = new();
        private readonly List<OrchesterEigentumId> _ausgeliehendesOrchesterEigentum = new();

        public string Vorname { get; private set; } = null!;
        public string Nachname { get; private set; } = null!;
        public Adresse Adresse { get; private set; } = null!;
        public DateTime Geburtstag { get; private set; }
        public string Telefonnummer { get; private set; } = null!;  
        public string Handynummer { get; private set; } = null!;
        public IReadOnlyList<Position> Positions => _positions.AsReadOnly();
        public Instrument DefaultInstrument { get; private set; } = null!;
        public Notenstimme DefaultNotenStimme { get; private set; }
        public IReadOnlyList<TerminRückmeldung> TerminRückmeldungen => _terminRückmeldungen.AsReadOnly();
        public IReadOnlyList<OrchesterEigentumId> AusgeliehendesOrchesterEigentum => _ausgeliehendesOrchesterEigentum.AsReadOnly();

        private OrchesterMitglied() { }
       
        private OrchesterMitglied(OrchesterMitgliedsId id, string vorname, string nachname, Adresse adresse, DateTime geburtstag, string telefonnummer, string handynummer, Instrument defaultInstrument, Notenstimme defaultNotenStimme): base(id)
        {
            Vorname = vorname;
            Nachname = nachname;
            Adresse = adresse;
            Geburtstag = geburtstag;
            Telefonnummer = telefonnummer;
            Handynummer = handynummer;
            DefaultInstrument = defaultInstrument;
            DefaultNotenStimme = defaultNotenStimme;
        }

        public static OrchesterMitglied Create(string vorname, string nachname, Adresse adresse, DateTime geburtstag, string telefonnummer, string handynummer, Instrument defaultInstrument, Notenstimme defaultNotenStimme)
        {
            return new OrchesterMitglied(OrchesterMitgliedsId.CreateUnique(), vorname, nachname, adresse, geburtstag, telefonnummer, handynummer, defaultInstrument, defaultNotenStimme);
        }
    }
}
