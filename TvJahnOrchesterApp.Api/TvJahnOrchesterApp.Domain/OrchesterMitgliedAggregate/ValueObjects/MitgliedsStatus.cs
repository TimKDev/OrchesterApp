using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Enums;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects
{
    public class MitgliedsStatus : ValueObject
    {
        public MitgliedsStatusEnum MitgliedsStatusEnum { get; private set; }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return MitgliedsStatusEnum;
        }

        private MitgliedsStatus() { }

        private MitgliedsStatus(MitgliedsStatusEnum mitgliedsStatusEnum)
        {
            MitgliedsStatusEnum = mitgliedsStatusEnum;
        }

        public static MitgliedsStatus Create(MitgliedsStatusEnum mitgliedsStatusEnum)
        {
            return new MitgliedsStatus(mitgliedsStatusEnum);
        }
    }
}