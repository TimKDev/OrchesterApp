using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class TerminRückmeldungOrchestermitglied : Entity<RückgemeldetePersonId>
    {
        private List<InstrumentId> _instruments = new();
        private List<NotenstimmeId> _notenstimmen = new();

        public IReadOnlyList<InstrumentId> Instruments => _instruments.AsReadOnly();
        public IReadOnlyList<NotenstimmeId> Notenstimme => _notenstimmen.AsReadOnly();
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; }
        public Rückmeldungsart Zugesagt { get; private set; } = Rückmeldungsart.NichtZurückgemeldet;
        public string? KommentarZusage { get; private set; }
        public DateTime? LetzteRückmeldung { get; private set; }
        public OrchesterMitgliedsId? RückmeldungDurchAnderesOrchestermitglied { get; private set; }
        public bool IstAnwesend { get; private set; } = false;
        public string? KommentarAnwesenheit { get; private set; }

        private TerminRückmeldungOrchestermitglied() { }

        private TerminRückmeldungOrchestermitglied(RückgemeldetePersonId id, OrchesterMitgliedsId orchesterMitgliedsId, List<InstrumentId> instruments, List<NotenstimmeId> notenstimmen): base(id)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
            _instruments = instruments;
            _notenstimmen = notenstimmen;
        }

        public static TerminRückmeldungOrchestermitglied Create(OrchesterMitgliedsId orchesterMitgliedsId, List<InstrumentId> defaultInstruments, List<NotenstimmeId> defaultNotenstimmen)
        {
            return new TerminRückmeldungOrchestermitglied(RückgemeldetePersonId.CreateUnique(), orchesterMitgliedsId, defaultInstruments, defaultNotenstimmen);
        }

        public void ChangeZusage(bool zugesagt, string? kommentar = null, OrchesterMitgliedsId otherOrchesterId = null)
        {
            Zugesagt = zugesagt ? Rückmeldungsart.Zugesagt : Rückmeldungsart.Abgesagt;
            LetzteRückmeldung = DateTime.Now;
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

        public void ChangeInstruments(List<InstrumentId> instruments)
        {
            _instruments = instruments;
        }

        public void ChangeNotenstimme(List<NotenstimmeId> notenstimmen)
        {
            _notenstimmen = notenstimmen;
        }
    }
}
