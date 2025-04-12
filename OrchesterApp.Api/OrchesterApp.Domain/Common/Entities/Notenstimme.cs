using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
{
    public sealed class Notenstimme : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private Notenstimme() { }

        private Notenstimme(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Notenstimme Create(int id, string value)
        {
            return new Notenstimme(id, value);
        }
    }
}
