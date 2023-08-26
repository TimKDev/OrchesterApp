using System.Security;

namespace TvJahnOrchesterApp.Contracts.OrchestraMembers
{
    public class OrchestraMemberContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] InstrumentIds { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }

        public OrchestraMemberContract(string firstName, string lastName, int[] orchestraInstrumentIds, Address address, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            InstrumentIds = orchestraInstrumentIds;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName);
        }
    }

    public record Address(
        string Street,
        string HouseNumber,
        string Zip,
        string City
    );
}