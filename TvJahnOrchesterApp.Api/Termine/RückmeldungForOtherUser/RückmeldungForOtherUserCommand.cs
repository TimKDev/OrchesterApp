using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungForOtherUser
{
    public record RückmeldungForOtherUserCommand(Guid TerminId, Guid OrchesterMitgliedsId, bool Zugesagt, string? Kommentar): IRequest<RückmeldungsResponse>;
}
