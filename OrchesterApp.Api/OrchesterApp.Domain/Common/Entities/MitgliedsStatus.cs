using OrchesterApp.Domain.Common.Interfaces;
using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.Common.Entities
{
    public sealed class MitgliedsStatus : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private MitgliedsStatus() { }

        private MitgliedsStatus(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static MitgliedsStatus Create(int id, string value)
        {
            return new MitgliedsStatus(id, value);
        }
    }
}
