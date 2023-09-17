using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungChangeInstrumentsAndNotes
{
    public record RückmeldungChangeInstrumentsAndNotesCommand(Guid TerminId, Guid RückmeldungsId, InstrumentDto[] Instruments, Notenstimme[] Notenstimme): IRequest<Unit>;
}
