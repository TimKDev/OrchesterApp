using TvJahnOrchesterApp.Application.Common.Interfaces.Dto;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface IOrchesterMitgliedRepository
    {
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> GetAllAsync(CancellationToken cancellationToken);
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied> CreateAsync(OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied orchesterMitglied, CancellationToken cancellationToken);
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied?> GetByNameAsync(string vorname, string lastname, CancellationToken cancellationToken);
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied?> GetByRegistrationKeyAsync(string registrationKey, CancellationToken cancellationToken);
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied> GetByIdAsync(OrchesterMitgliedsId id, CancellationToken cancellationToken);

        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied?> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> QueryByIdAsync(OrchesterMitgliedsId[] ids, CancellationToken cancellationToken);
        Task DeleteByIdAsync(OrchesterMitgliedsId orchesterMitgliedsId, CancellationToken cancellationToken);
        Task<OrchesterMitgliedWithName[]> GetAllNames(CancellationToken cancellationToken);
        Task<OrchesterMitgliedAdminInfo[]> GetAllAdminInfo(CancellationToken cancellationToken);
    }
}
