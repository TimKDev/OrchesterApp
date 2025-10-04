using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Task<OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied> GetCurrentOrchesterMitgliedAsync(
            CancellationToken cancellationToken);

        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

        Task<bool> IsUserVorstand(CancellationToken cancellationToken);

        Task<UserId> GetCurrentUserIdAsync(CancellationToken cancellationToken);
    }
}