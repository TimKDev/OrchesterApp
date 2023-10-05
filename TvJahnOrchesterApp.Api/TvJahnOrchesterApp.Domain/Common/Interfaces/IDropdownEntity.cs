using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Interfaces
{
    public interface IDropdownEntity
    {
        public int Id { get; }
        public string Value { get; }
    }
}