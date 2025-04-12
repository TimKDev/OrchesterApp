using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class EinsatzplanNotenMapping : Entity<int>
    {
        public int NotenId { get; private set; }

        private EinsatzplanNotenMapping() { }

        private EinsatzplanNotenMapping(int notenId)
        {
            NotenId = notenId;
        }

        public static EinsatzplanNotenMapping Create(int notenId)
        {
            return new EinsatzplanNotenMapping(notenId);
        }
    }
}
