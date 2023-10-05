using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Entities
{
    public sealed class OrchesterMitgliedPositionsMapping: Entity<OrchesterMitgliedPositionsMappingId> 
    {
        public int PositionId { get; private set; }

        private OrchesterMitgliedPositionsMapping() { }

        private OrchesterMitgliedPositionsMapping(int positionId)
        {
            PositionId = positionId;
        }

        public static OrchesterMitgliedPositionsMapping Create(int positionId)
        {
            return new OrchesterMitgliedPositionsMapping(positionId);
        }
    }
}
