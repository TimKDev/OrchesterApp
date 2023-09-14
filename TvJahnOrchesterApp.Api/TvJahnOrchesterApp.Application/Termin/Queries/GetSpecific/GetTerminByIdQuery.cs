using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetSpecific
{
    public record GetTerminByIdQuery(Guid Id): IRequest<TerminResponse>;
}
