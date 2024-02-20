using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class TerminRückmeldungOrchestermitglied : Entity<RückgemeldetePersonId>
    {
        private List<TerminRückmeldungInstrumentMapping> _terminRückmeldungInstrumentMappings = new();
        private List<TerminRückmeldungNotenstimmenMapping> _terminRückmeldungNotenstimmenMappings = new();

        public IReadOnlyList<TerminRückmeldungInstrumentMapping> TerminRückmeldungInstrumentMappings => _terminRückmeldungInstrumentMappings.AsReadOnly();
        public IReadOnlyList<TerminRückmeldungNotenstimmenMapping> TerminRückmeldungNotenstimmenMappings => _terminRückmeldungNotenstimmenMappings.AsReadOnly();
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; }
        public int Zugesagt { get; private set; } = (int)RückmeldungsartEnum.NichtZurückgemeldet;
        public string? KommentarZusage { get; private set; }
        public DateTime? LetzteRückmeldung { get; private set; }
        public OrchesterMitgliedsId? RückmeldungDurchAnderesOrchestermitglied { get; private set; }
        public bool IstAnwesend { get; private set; } = false;
        public string? KommentarAnwesenheit { get; private set; }

        private TerminRückmeldungOrchestermitglied() { }

        private TerminRückmeldungOrchestermitglied(RückgemeldetePersonId id, OrchesterMitgliedsId orchesterMitgliedsId): base(id)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
        }

        public static TerminRückmeldungOrchestermitglied Create(OrchesterMitgliedsId orchesterMitgliedsId, List<int?> defaultInstruments, List<int?> defaultNotenstimmen)
        {
            return new TerminRückmeldungOrchestermitglied(RückgemeldetePersonId.CreateUnique(), orchesterMitgliedsId);
        }

        public void ChangeZusage(int zugesagt, string? kommentar = null, OrchesterMitgliedsId otherOrchesterId = null)
        {
            Zugesagt = zugesagt;
            LetzteRückmeldung = DateTime.UtcNow;
            KommentarZusage = kommentar;
            if(otherOrchesterId is not null)
            {
                RückmeldungDurchAnderesOrchestermitglied = otherOrchesterId;
            }
        }

        public void ChangeAnwesenheit(bool istAnwesend, string? kommentar = null)
        {
            IstAnwesend = istAnwesend;
            KommentarAnwesenheit = kommentar;
        }
    }
}
