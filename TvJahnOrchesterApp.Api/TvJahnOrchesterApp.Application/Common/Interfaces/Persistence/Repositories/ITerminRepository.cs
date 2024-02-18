using TvJahnOrchesterApp.Application.Common.Interfaces.Dto;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface ITerminRepository
    {
        public Task<Domain.TerminAggregate.Termin> Save(Domain.TerminAggregate.Termin termin, CancellationToken cancellationToken);

        public Task<Domain.TerminAggregate.Termin[]> GetAll(CancellationToken cancellationToken);

        public Task<Domain.TerminAggregate.Termin> GetById(Guid guid, CancellationToken cancellationToken);

        public Task<bool> Delete(Guid guid, CancellationToken cancellationToken);
        Task<TerminWithResponses[]> GetTerminResponsesInYear(int year, CancellationToken cancellationToken);
    }
}
