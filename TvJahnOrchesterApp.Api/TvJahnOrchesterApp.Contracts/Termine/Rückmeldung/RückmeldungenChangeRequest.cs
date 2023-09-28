using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Contracts.Termine.Rückmeldung
{
    public record RückmeldungenChangeRequest(Guid RückmeldungsId, InstrumentDto[] Instruments, NotenstimmeEnum[] Notenstimme);
}
