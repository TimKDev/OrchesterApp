using TvJahnOrchesterApp.Contracts.Termine.Dto;

namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record GetRückmeldungenTerminResponse(Guid TerminId, string Name, TerminRückmeldungOrchestermitgliedDto[] TerminRückmeldungOrchesterMitglieder);
}
