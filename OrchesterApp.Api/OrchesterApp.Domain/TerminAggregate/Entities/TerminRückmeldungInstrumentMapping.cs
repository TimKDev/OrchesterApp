using OrchesterApp.Domain.Common.Models;

namespace OrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class TerminRückmeldungInstrumentMapping : Entity<int>
    {
        public int InstrumentId { get; private set; }

        private TerminRückmeldungInstrumentMapping() { }

        private TerminRückmeldungInstrumentMapping(int instrumentId)
        {
            InstrumentId = instrumentId;
        }

        public static TerminRückmeldungInstrumentMapping Create(int instrumentId)
        {
            return new TerminRückmeldungInstrumentMapping(instrumentId);
        }
    }
}
