using MediatR;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetSpecific
{
    public record GetTerminByIdQuery(Guid Id): IRequest<Domain.TerminAggregate.Termin>;
}
