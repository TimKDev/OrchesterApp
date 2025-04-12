using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class EinsatzplanUniformMapping : Entity<int>
    {
        public int UniformId { get; private set; }

        private EinsatzplanUniformMapping() { }

        private EinsatzplanUniformMapping(int uniformId)
        {
            UniformId = uniformId;
        }

        public static EinsatzplanUniformMapping Create(int uniformId)
        {
            return new EinsatzplanUniformMapping(uniformId);
        }
    }
}
