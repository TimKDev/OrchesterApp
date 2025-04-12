using OrchesterApp.Domain.Common.Models;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace OrchesterApp.Domain.OrchesterMitgliedAggregate.Entities
{
    public sealed class OrchesterMitgliedPositionsMapping: Entity<OrchesterMitgliedPositionsMappingId> 
    {
        public int PositionId { get; private set; }

        private OrchesterMitgliedPositionsMapping() { }

        private OrchesterMitgliedPositionsMapping(OrchesterMitgliedPositionsMappingId id, int positionId): base(id)
        {
            PositionId = positionId;
        }

        public static OrchesterMitgliedPositionsMapping Create(int positionId)
        {
            return new OrchesterMitgliedPositionsMapping(OrchesterMitgliedPositionsMappingId.CreateUnique(), positionId);
        }
    }
}
