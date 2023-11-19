using FluentAssertions;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;

namespace TvJahnOrchesterApp.Domain.Tests
{
    public class OrchesterMitgliedTests
    {
        private static readonly string _vorname = "Tim";
        private static readonly string _nachname = "Kempkens";
        private static readonly string _straﬂe = "M¸lhausener Straﬂe";
        private static readonly string _hausnummer = "68";
        private static readonly string _zip = "47839";
        private static readonly string _stadt = "Krefeld";
        private static readonly Adresse _adresse = Adresse.Create(_straﬂe, _hausnummer, _zip, _stadt);
        private static readonly DateTime? _geburtstag = DateTime.Parse("1998-01-24");
        private static readonly string? _telefonnummer = "04284474";
        private static readonly string? _handynummer = "7473920100";
        private static readonly int? _defaultInstrument = 0;
        private static readonly int? _defaultNotenStimme = 1;
        private static readonly int _mitgliedsStatus = 1;
        private static readonly string _registrationKey = "Test123";

        private readonly OrchesterMitglied _orchesterMitglied = OrchesterMitglied.Create(_vorname, _nachname, _adresse, _geburtstag, _telefonnummer, _handynummer, _defaultInstrument, _defaultNotenStimme, _registrationKey, _mitgliedsStatus);

        [Fact]
        public void Create_GivenInput_ShouldInitialiseMitgliedsInfo()
        {
            // Arrange
            var sut = _orchesterMitglied;

            //Act

            //Asssert
            sut.Id.Value.Should().NotBe(new Guid());
            sut.Vorname.Should().Be(_vorname);
            sut.Nachname.Should().Be(_nachname);
            sut.Geburtstag.Should().Be(_geburtstag);
            sut.Telefonnummer.Should().Be(_telefonnummer);
            sut.Handynummer.Should().Be(_handynummer);
            sut.DefaultInstrument.Should().Be(_defaultInstrument);
            sut.DefaultNotenStimme.Should().Be(_defaultNotenStimme);
            sut.OrchesterMitgliedsStatus.Should().Be(_mitgliedsStatus);
        }

        [Fact]
        public void Create_GivenInput_ShouldInitialiseAddress()
        {
            // Arrange
            var sut = _orchesterMitglied;

            //Act

            //Asssert
            sut.Adresse.Straﬂe.Should().Be(_straﬂe);
            sut.Adresse.Hausnummer.Should().Be(_hausnummer);
            sut.Adresse.Postleitzahl.Should().Be(_zip);
            sut.Adresse.Stadt.Should().Be(_stadt);
            sut.Adresse.Longitide.Should().BeNull();
            sut.Adresse.Latitude.Should().BeNull();
            sut.Adresse.Zusatz.Should().BeNull();
        }

        [Fact]
        public void Create_GivenInput_ShouldInitialiseEmptyPositionsArray()
        {
            // Arrange
            var sut = _orchesterMitglied;

            //Act

            //Asssert
            sut.PositionMappings.Should().HaveCount(0);
        }

        [Fact]
        public void Create_GivenInput_ShouldInitialiseRegisterKeyAndSetExpirationDate()
        {
            // Arrange
            var sut = _orchesterMitglied;

            //Act

            //Asssert
            sut.RegisterKey.Should().Be(_registrationKey);
            sut.RegisterKeyExpirationDate.Should().BeAfter(DateTime.Now.AddDays(10).AddMinutes(-1));
        }

        [Fact]
        public void Create_GivenInput_ShouldInitialiseUserInfoAsNull()
        {
            // Arrange
            var sut = _orchesterMitglied;

            //Act

            //Asssert
            sut.ConnectedUserId.Should().BeNull();
            sut.UserFirstConnected.Should().BeNull();
            sut.UserLastLogin.Should().BeNull();
            sut.MemberSince.Should().BeNull();
            sut.MemberSinceInYears.Should().BeNull();
        }
    }
}