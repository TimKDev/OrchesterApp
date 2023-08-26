using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.UserAggregate
{
    public sealed class User: AggregateRoot<UserId, Guid>
    {
        private readonly List<AbstimmungsId> _abstimmungsIds = new();

        public string Email { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; } = null!;
        public IReadOnlyList<AbstimmungsId> AbstimmungsIds => _abstimmungsIds.AsReadOnly();

        private User() { }

        private User(UserId id, string email, string password, OrchesterMitgliedsId orchesterMitgliedsId): base(id)
        {
            Email = email;
            Password = password;
            OrchesterMitgliedsId = orchesterMitgliedsId;
        }

        public static User Create(string email, string password, OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return new User(UserId.CreateUnique(), email, password, orchesterMitgliedsId);
        }
    }
}
