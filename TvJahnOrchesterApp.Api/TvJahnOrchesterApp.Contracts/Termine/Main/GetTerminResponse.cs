using TvJahnOrchesterApp.Contracts.Termine.Dto;

namespace TvJahnOrchesterApp.Contracts.Termine.Main
{
    public record GetTerminResponse(Guid TerminId, string Name, int TerminArt, EinsatzPlanDto EinsatzPlan, TerminRückmeldungOrchestermitgliedDto UserRückmeldung);
}

