using OrchesterApp.Domain.Common.Entities;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface IInstrumentRepository
    {
        Task<Instrument> GetByIdAsync(int Id, CancellationToken cancellationToken);
    }
}