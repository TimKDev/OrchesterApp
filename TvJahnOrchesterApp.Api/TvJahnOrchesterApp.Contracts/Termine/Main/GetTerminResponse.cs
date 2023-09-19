using TvJahnOrchesterApp.Contracts.Termine.Dto;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record GetTerminResponse(Guid TerminId, string Name, TerminArt TerminArt, EinsatzPlanDto EinsatzPlan, TerminRückmeldungOrchestermitgliedDto UserRückmeldung);
}

