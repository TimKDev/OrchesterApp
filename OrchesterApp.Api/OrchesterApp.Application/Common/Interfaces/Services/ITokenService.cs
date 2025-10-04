using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(User user);
        string GenerateRefreshToken();
        Task AddRefreshTokenToUserInDBAsync(User user, string refreshToken);
        bool IsRefreshTokenValid(User user, string refreshToken);
        Task RevokeRefreshTokenAsync(string username);
    }
}