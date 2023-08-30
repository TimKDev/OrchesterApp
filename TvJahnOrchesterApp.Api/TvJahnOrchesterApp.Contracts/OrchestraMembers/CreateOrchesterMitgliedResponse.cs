namespace TvJahnOrchesterApp.Contracts.OrchestraMembers
{
    public record CreateOrchesterMitgliedResponse(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, List<PositionDto> Positions, InstrumentDto DefaultInstrument, NotenstimmeDto DefaultNotenStimme, List<TerminIdDto> ZugesagteTermine, List<TerminIdDto> AbgesagteTermine, List<TerminIdDto> NichtZurückgemeldeteTermine, OrchesterEigentumIdDto[] AusgeliehendesOrchesterEigentum);
}
