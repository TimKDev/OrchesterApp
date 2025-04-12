using TvJahnOrchesterApp.Application.Common.Interfaces.Dto;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface ITerminRepository
    {
        public Task<OrchesterApp.Domain.TerminAggregate.Termin> Save(OrchesterApp.Domain.TerminAggregate.Termin termin, CancellationToken cancellationToken);

        public Task<OrchesterApp.Domain.TerminAggregate.Termin[]> GetAll(CancellationToken cancellationToken);

        public Task<OrchesterApp.Domain.TerminAggregate.Termin> GetById(Guid guid, CancellationToken cancellationToken);

        public Task<bool> Delete(Guid guid, CancellationToken cancellationToken);
        Task<TerminWithResponses[]> GetTerminResponsesInYearAndPast(int year, CancellationToken cancellationToken);
    }
}
