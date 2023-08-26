using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.OrchesterEigentum.ValueObjects
{
    public sealed class OrchesterEigentumId : AggregateRootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private OrchesterEigentumId() { }

        private OrchesterEigentumId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static OrchesterEigentumId CreateUnique() 
        { 
            return new OrchesterEigentumId(Guid.NewGuid());
        }
    }
}
