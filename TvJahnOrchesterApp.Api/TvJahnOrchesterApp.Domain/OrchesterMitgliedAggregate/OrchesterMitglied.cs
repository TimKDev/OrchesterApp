using System.Security.Cryptography;
using System.Text;
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
        private const int RegistrationKeyExpireDays = 10;

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
        public string? RegisterKey { get; private set; }
        public DateTime RegisterKeyExpirationDate { get; private set; }
        public string? ConnectedUserId { get; private set; }
        public DateTime UserFirstConnected { get; private set; }

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

        public void SetRegisterKey(string key)
        {
            RegisterKey = GetHashString(key);
            RegisterKeyExpirationDate = DateTime.Now.AddDays(RegistrationKeyExpireDays);
        }

        public bool UseRegisterKey(string userId, string key)
        {
            if(ConnectedUserId is null)
            {
                throw new InvalidOperationException("Für dieses Orchestermitglied wurde bereits ein User verknüpft.");
            }
            if(RegisterKey == GetHashString(key) && RegisterKeyExpirationDate <= DateTime.Now)
            {
                ConnectedUserId = userId;
                RegisterKey = null;
                UserFirstConnected = DateTime.Now;
                return true;
            }
            return false;
        }

        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in SHA256.HashData(Encoding.UTF8.GetBytes(inputString)))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
