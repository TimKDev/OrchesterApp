using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
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
