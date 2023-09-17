using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungenChangeResponse(Guid RückmeldungsId, InstrumentDto[] Instruments, Notenstimme[] Notenstimme);
}
