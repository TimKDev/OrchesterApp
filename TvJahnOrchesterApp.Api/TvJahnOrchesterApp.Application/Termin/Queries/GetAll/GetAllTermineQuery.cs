using MediatR;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAll
{
    public record GetAllTermineQuery(): IRequest<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied?)[]>;
}
