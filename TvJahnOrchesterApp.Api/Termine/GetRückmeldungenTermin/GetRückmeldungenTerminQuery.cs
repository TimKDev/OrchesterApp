using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetRückmeldungenTermin
{
    public record GetRückmeldungenTerminQuery(Guid TerminId): IRequest<TerminRückmeldungsResponse>;
}
