using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.OrchestraMembers
{
    public record CreateOrchesterMitgliedResponse(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, List<Position> Positions, InstrumentDto DefaultInstrument, Notenstimme DefaultNotenStimme, List<TerminIdDto> ZugesagteTermine, List<TerminIdDto> AbgesagteTermine, List<TerminIdDto> NichtZurückgemeldeteTermine, OrchesterEigentumIdDto[] AusgeliehendesOrchesterEigentum);
}
