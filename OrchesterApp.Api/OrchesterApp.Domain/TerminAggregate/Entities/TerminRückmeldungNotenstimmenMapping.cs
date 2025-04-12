using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class TerminRückmeldungNotenstimmenMapping : Entity<int>
    {
        public int NotenstimmenId { get; private set; }

        private TerminRückmeldungNotenstimmenMapping() { }

        private TerminRückmeldungNotenstimmenMapping(int notenstimmenId)
        {
            NotenstimmenId = notenstimmenId;
        }

        public static TerminRückmeldungNotenstimmenMapping Create(int notenstimmenId)
        {
            return new TerminRückmeldungNotenstimmenMapping(notenstimmenId);
        }
    }
}
