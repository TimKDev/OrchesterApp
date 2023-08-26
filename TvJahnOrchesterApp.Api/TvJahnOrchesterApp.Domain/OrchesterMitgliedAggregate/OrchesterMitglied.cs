using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterEigentum.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate
{
    public sealed class OrchesterMitglied: AggregateRoot<OrchesterMitgliedsId, Guid>
    {
        private readonly List<TerminId> _zugesagteTermine = new();
        private readonly List<TerminId> _abgesagteTermine = new();
        private readonly List<TerminId> _nichtZurückgemeldeteTermine = new();

        public string Vorname { get; private set; } = null!;
        public string Nachname { get; private set; } = null!;
        public Adresse Adresse { get; private set; } = null!;
        public DateTime Geburtstag { get; private set; }
        public string Telefonnummer { get; private set; } = null!;  
        public string Handynummer { get; private set; } = null!;
        public Instrument DefaultInstrument { get; private set; } = null!;
        public Notenstimme DefaultNotenStimme { get; private set; }
        public IReadOnlyList<TerminId> ZugesagteTermine => _zugesagteTermine.AsReadOnly();
        public IReadOnlyList<TerminId> AbgesagteTermine => _abgesagteTermine.AsReadOnly();
        public IReadOnlyList<TerminId> NichtZurückgemeldeteTermine => _nichtZurückgemeldeteTermine.AsReadOnly(); 
        public OrchesterEigentumId[] AusgeliehendesOrchesterEigentum { get; private set; } = null!;

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
