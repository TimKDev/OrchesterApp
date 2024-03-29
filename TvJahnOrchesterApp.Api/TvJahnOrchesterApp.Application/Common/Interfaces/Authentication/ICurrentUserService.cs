using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Authentication
{
    public interface ICurrentUserService
    {
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied> GetCurrentOrchesterMitgliedAsync(CancellationToken cancellationToken);

        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

        Task<bool> IsUserVorstand(CancellationToken cancellationToken);
    }
}
