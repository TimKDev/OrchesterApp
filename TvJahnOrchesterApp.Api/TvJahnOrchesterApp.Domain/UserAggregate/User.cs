using Microsoft.AspNetCore.Identity;
using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.UserAggregate
{
    public sealed class User: IdentityUser
    {
        private const int RefreshTokenExpireDays = 7;
        private readonly List<AbstimmungsId> _abstimmungsIds = new();

        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; } = null!;
        public IReadOnlyList<AbstimmungsId> AbstimmungsIds => _abstimmungsIds.AsReadOnly();
        public string? RefreshToken { get; private set; }
        public DateTime RefreshTokenExpiryTime { get; private set; }

        private User() { }

        private User(UserId id, OrchesterMitgliedsId orchesterMitgliedsId, string email)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
            Email = email;
        }

        public static User Create(OrchesterMitgliedsId orchesterMitgliedsId, string email)
        {
            return new User(UserId.CreateUnique(), orchesterMitgliedsId, email);
        }

        public void SetRefreshToken(string? refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpireDays);
        }
    }
}
