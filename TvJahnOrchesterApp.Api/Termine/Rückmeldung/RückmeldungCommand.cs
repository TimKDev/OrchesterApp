using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung
{
    public record RückmeldungCommand(Guid TerminId, bool Zugesagt, string? Kommentar): IRequest<RückmeldungsResponse>;
}
