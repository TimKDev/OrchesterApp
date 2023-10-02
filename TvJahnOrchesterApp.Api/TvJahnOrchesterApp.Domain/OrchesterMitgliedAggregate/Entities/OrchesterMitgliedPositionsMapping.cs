using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Entities
{
    public sealed class OrchesterMitgliedPositionsMapping: Entity<OrchesterMitgliedPositionsMappingId> 
    {
        public PositionId PositionId { get; private set; }

        private OrchesterMitgliedPositionsMapping() { }

        private OrchesterMitgliedPositionsMapping(PositionId positionId)
        {
            PositionId = positionId;
        }

        public static OrchesterMitgliedPositionsMapping Create(PositionId positionId)
        {
            return new OrchesterMitgliedPositionsMapping(positionId);
        }
    }
}
