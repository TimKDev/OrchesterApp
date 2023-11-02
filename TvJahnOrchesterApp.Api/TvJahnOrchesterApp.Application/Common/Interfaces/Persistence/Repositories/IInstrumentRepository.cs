using TvJahnOrchesterApp.Domain.Common.Entities;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories
{
    public interface IInstrumentRepository
    {
        Task<Instrument> GetByIdAsync(int Id, CancellationToken cancellationToken);
    }
}