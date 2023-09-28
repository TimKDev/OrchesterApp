using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Contracts.OrchestraMembers
{
    public record CreateOrchesterMitgliedRequest(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, InstrumentDto DefaultInstrument, NotenstimmeEnum DefaultNotenStimme, string RegisterKey);
}