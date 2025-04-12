using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
{
    public sealed class Uniform : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private Uniform() { }

        private Uniform(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Uniform Create(int id, string value)
        {
            return new Uniform(id, value);
        }
    }
}
