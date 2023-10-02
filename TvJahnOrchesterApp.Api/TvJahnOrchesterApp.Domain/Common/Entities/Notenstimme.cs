using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class Notenstimme : Entity<NotenstimmeId>, IDropdownEntity<NotenstimmeId>
    {
        public string Value { get; private set; } = null!;

        private Notenstimme() { }

        private Notenstimme(NotenstimmeId id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Notenstimme Create(int id, string value)
        {
            return new Notenstimme(NotenstimmeId.Create(id), value);
        }
    }
}
