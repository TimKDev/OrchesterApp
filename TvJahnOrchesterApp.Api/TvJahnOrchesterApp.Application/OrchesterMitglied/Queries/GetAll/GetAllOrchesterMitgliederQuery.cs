using MediatR;

namespace TvJahnOrchesterApp.Application.OrchesterMitglied.Queries.GetAll
{
    public record GetAllOrchesterMitgliederQuery: IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> { };
}
