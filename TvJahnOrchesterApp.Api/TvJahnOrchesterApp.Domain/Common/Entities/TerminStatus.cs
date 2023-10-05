using TvJahnOrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.Common.Entities
{
    public sealed class TerminStatus : Entity<int>, IDropdownEntity
    {
        public string Value { get; private set; } = null!;

        private TerminStatus() { }

        private TerminStatus(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static TerminStatus Create(int id, string value)
        {
            return new TerminStatus(id, value);
        }
    }
}
