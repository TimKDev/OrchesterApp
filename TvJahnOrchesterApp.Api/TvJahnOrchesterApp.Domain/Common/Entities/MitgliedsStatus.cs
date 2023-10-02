using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class MitgliedsStatus : Entity<MitgliedsStatusId>, IDropdownEntity<MitgliedsStatusId>
    {
        public string Value { get; private set; } = null!;

        private MitgliedsStatus() { }

        private MitgliedsStatus(MitgliedsStatusId id, string value)
        {
            Id = id;
            Value = value;
        }

        public static MitgliedsStatus Create(int id, string value)
        {
            return new MitgliedsStatus(MitgliedsStatusId.Create(id), value);
        }
    }
}
