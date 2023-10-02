using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungenChangeResponse(Guid RückmeldungsId, int[] Instruments, int[] Notenstimme);
}
