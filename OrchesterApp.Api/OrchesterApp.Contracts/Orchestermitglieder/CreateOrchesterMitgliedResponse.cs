using TvJahnOrchesterApp.Contracts.Common.Dto;
using TvJahnOrchesterApp.Contracts.Orchestermitglieder.Dto;

namespace TvJahnOrchesterApp.Contracts.Orchestermitglieder
{
    public record CreateOrchesterMitgliedResponse(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int[] Positions, int DefaultInstrument, int DefaultNotenStimme, List<TerminIdDto> ZugesagteTermine, List<TerminIdDto> AbgesagteTermine, List<TerminIdDto> NichtZurückgemeldeteTermine, OrchesterEigentumIdDto[] AusgeliehendesOrchesterEigentum);
}
