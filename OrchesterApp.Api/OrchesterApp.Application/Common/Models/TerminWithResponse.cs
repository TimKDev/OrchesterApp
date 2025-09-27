using OrchesterApp.Domain.TerminAggregate.Entities;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Models
{
    public record TerminWithResponses(
        TerminId Id,
        int? TerminArt,
        IReadOnlyList<TerminRückmeldungOrchestermitglied> RückmeldungOrchestermitglieder);
}