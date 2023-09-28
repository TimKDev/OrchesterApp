using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.OrchestraMembers
{
    public record CreateOrchesterMitgliedResponse(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, List<Position> Positions, InstrumentDto DefaultInstrument, NotenstimmeEnum DefaultNotenStimme, List<TerminIdDto> ZugesagteTermine, List<TerminIdDto> AbgesagteTermine, List<TerminIdDto> NichtZurückgemeldeteTermine, OrchesterEigentumIdDto[] AusgeliehendesOrchesterEigentum);
}
