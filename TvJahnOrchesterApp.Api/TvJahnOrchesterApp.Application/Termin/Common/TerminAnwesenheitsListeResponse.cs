using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Common
{
    public record TerminAnwesenheitsListeResponse(TerminAnwesenheitsListenEintrag[] TerminAnwesenheitsListe);

    public record TerminAnwesenheitsListenEintrag(string Vorname, string Nachname, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied);
}


    
