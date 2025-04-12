using OrchesterApp.Domain.Common.ValueObjects;

namespace OrchesterApp.Domain.Common.Interfaces
{
    public interface IDropdownEntity
    {
        public int Id { get; }
        public string Value { get; }
    }
}