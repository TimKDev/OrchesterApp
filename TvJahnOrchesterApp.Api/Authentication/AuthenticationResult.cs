using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
