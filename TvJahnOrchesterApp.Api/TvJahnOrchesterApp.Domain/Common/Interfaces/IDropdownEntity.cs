using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Domain.Common.Interfaces
{
    public interface IDropdownEntity<T> where T : IDropdownId
    {
        public T Id { get; }
        public string Value { get; }
    }
}