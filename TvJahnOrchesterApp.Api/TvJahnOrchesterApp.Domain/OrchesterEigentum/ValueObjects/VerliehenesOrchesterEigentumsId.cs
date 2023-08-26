using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.OrchesterEigentum.ValueObjects
{
    public sealed class VerliehenesOrchesterEigentumsId: ValueObject
    {
        public Guid Value { get; private set; }

        private VerliehenesOrchesterEigentumsId() { }

        private VerliehenesOrchesterEigentumsId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static VerliehenesOrchesterEigentumsId Create()
        {
            return new VerliehenesOrchesterEigentumsId(Guid.NewGuid());
        }
    }
}
