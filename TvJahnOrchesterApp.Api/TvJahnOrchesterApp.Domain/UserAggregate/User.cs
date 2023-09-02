using Microsoft.AspNetCore.Identity;
using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.UserAggregate
{
    public sealed class User: IdentityUser
    {
        private readonly List<AbstimmungsId> _abstimmungsIds = new();
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; } = null!;
        public IReadOnlyList<AbstimmungsId> AbstimmungsIds => _abstimmungsIds.AsReadOnly();

        private User() { }

        private User(UserId id, OrchesterMitgliedsId orchesterMitgliedsId)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
        }

        public static User Create(OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return new User(UserId.CreateUnique(), orchesterMitgliedsId);
        }
    }
}
