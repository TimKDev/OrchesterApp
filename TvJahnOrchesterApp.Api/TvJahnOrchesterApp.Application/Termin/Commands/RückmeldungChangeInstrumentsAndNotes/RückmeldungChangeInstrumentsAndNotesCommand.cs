using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungChangeInstrumentsAndNotes
{
    public record RückmeldungChangeInstrumentsAndNotesCommand(Guid TerminId, Guid RückmeldungsId, InstrumentDto[] Instruments, NotenstimmeEnum[] Notenstimme): IRequest<Unit>;
}
