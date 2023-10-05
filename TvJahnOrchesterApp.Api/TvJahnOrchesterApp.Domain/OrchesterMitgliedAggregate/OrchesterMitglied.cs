using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Entities;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate
{
    public sealed class OrchesterMitglied: AggregateRoot<OrchesterMitgliedsId, Guid>
    {
        private const int RegistrationKeyExpireDays = 10;

        private readonly List<OrchesterMitgliedPositionsMapping> _positionMappings = new();
        //private readonly List<OrchesterEigentumId> _ausgeliehendesOrchesterEigentum = new();

        public string Vorname { get; private set; } = null!;
        public string Nachname { get; private set; } = null!;
        public Adresse Adresse { get; private set; } = null!;
        public DateTime Geburtstag { get; private set; }
        public string Telefonnummer { get; private set; } = null!;  
        public string Handynummer { get; private set; } = null!;
        public IReadOnlyList<OrchesterMitgliedPositionsMapping> PositionMappings => _positionMappings.AsReadOnly();
        public int? DefaultInstrument { get; private set; } = null!;
        public int? DefaultNotenStimme { get; private set; } = null!;
        //public IReadOnlyList<OrchesterEigentumId> AusgeliehendesOrchesterEigentum => _ausgeliehendesOrchesterEigentum.AsReadOnly();
        public string RegisterKey { get; private set; } = null!;
        public DateTime RegisterKeyExpirationDate { get; private set; }
        public string? ConnectedUserId { get; private set; }
        public DateTime? UserFirstConnected { get; private set; }
        public DateTime? UserLastLogin { get; private set; }
        public int? OrchesterMitgliedsStatus { get; private set; }

        private OrchesterMitglied() { }
       
        private OrchesterMitglied(OrchesterMitgliedsId id, string vorname, string nachname, Adresse adresse, DateTime geburtstag, string telefonnummer, string handynummer, int defaultInstrument, int defaultNotenStimme, int mitgliedsStatus, string registrationKey) : base(id)
        {
            Vorname = vorname;
            Nachname = nachname;
            Adresse = adresse;
            Geburtstag = geburtstag;
            Telefonnummer = telefonnummer;
            Handynummer = handynummer;
            DefaultInstrument = defaultInstrument;
            DefaultNotenStimme = defaultNotenStimme;
            RegisterKey = registrationKey;
            OrchesterMitgliedsStatus = mitgliedsStatus;
            RegisterKeyExpirationDate = DateTime.Now.AddDays(RegistrationKeyExpireDays);
        }

        public static OrchesterMitglied Create(string vorname, string nachname, Adresse adresse, DateTime geburtstag, string telefonnummer, string handynummer, int defaultInstrument, int defaultNotenStimme, string registrationKey, int mitgliedsStatus)
        {
            return new OrchesterMitglied(OrchesterMitgliedsId.CreateUnique(), vorname, nachname, adresse, geburtstag, telefonnummer, handynummer, defaultInstrument, defaultNotenStimme, mitgliedsStatus, registrationKey);
        }

        public void SetRegisterKey(string key)
        {
            RegisterKey = key;
            RegisterKeyExpirationDate = DateTime.Now.AddDays(RegistrationKeyExpireDays);
        }

        public void ConnectWithUser(string userId)
        {
            ConnectedUserId = userId;
            UserFirstConnected = DateTime.Now;
        }

        public bool ValidateRegistrationKey(string key)
        {
            return ConnectedUserId is null && RegisterKey == key && DateTime.Now <= RegisterKeyExpirationDate;
        }

        public void ChangeMitgliedsStatus(int mitgliedsStatus)
        {
            OrchesterMitgliedsStatus = mitgliedsStatus;
        } 

        public void RemoveUser()
        {
            ConnectedUserId = null;
            UserFirstConnected = null;
            UserLastLogin = null;
        }

        public void UserLogin()
        {
            UserLastLogin = DateTime.Now;
        }
    }
}
