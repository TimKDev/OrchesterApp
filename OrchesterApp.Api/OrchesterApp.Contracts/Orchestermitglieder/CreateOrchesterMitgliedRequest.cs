using TvJahnOrchesterApp.Contracts.Common.Dto;

namespace TvJahnOrchesterApp.Contracts.Orchestermitglieder
{
    public record CreateOrchesterMitgliedRequest(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int DefaultInstrument, int DefaultNotenStimme, string RegisterKey);
}