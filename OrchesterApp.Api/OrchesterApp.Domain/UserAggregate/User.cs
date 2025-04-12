using Microsoft.AspNetCore.Identity;
using OrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.UserAggregate.ValueObjects;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.UserAggregate
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
