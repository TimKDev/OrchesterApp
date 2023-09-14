using MediatR;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAll
{
    public record GetAllTermineQuery(): IRequest<Domain.TerminAggregate.Termin[]>;
}
