using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface IOrchesterMitgliedRepository
    {
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> GetAllAsync(CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied> CreateAsync(Domain.OrchesterMitgliedAggregate.OrchesterMitglied orchesterMitglied, CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied?> GetByNameAsync(string vorname, string lastname, CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied?> GetByRegistrationKeyAsync(string registrationKey, CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied> GetByIdAsync(OrchesterMitgliedsId id, CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> QueryByIdAsync(OrchesterMitgliedsId[] ids, CancellationToken cancellationToken);
    }
}
