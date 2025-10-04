using OrchesterApp.Domain.TerminAggregate;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface ITerminRepository
    {
        Task<Termin> Save(Termin termin, CancellationToken cancellationToken);

        Task<Termin[]> GetAll(CancellationToken cancellationToken);
        Task<Termin[]> GetFutureTerminsAsync(CancellationToken cancellationToken);

        Task<Termin> GetById(Guid guid, CancellationToken cancellationToken);

        Task<bool> Delete(Guid guid, CancellationToken cancellationToken);
        Task<TerminWithResponses[]> GetTerminResponsesInYearAndPast(int year, CancellationToken cancellationToken);
    }
}