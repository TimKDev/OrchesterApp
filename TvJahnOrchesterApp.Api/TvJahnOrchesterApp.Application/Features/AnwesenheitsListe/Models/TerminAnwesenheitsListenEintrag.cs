using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Models
{
    public record TerminAnwesenheitsListenEintrag(string Vorname, string Nachname, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied);
}



