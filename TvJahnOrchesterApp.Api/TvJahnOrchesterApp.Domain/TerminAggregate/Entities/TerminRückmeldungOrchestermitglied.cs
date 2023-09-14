using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class TerminRückmeldungOrchestermitglied : Entity<RückgemeldetePersonId>
    {
        private List<Instrument> _instruments = new();
        private List<Notenstimme> _notenstimmen = new();

        public IReadOnlyList<Instrument> Instruments => _instruments.AsReadOnly();
        public IReadOnlyList<Notenstimme> Notenstimmme => _notenstimmen.AsReadOnly();
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; }
        public Rückmeldungsart Zugesagt { get; private set; } = Rückmeldungsart.NichtZurückgemeldet;
        public string? KommentarZusage { get; private set; }
        public DateTime? LetzteRückmeldung { get; private set; }
        public OrchesterMitgliedsId? RückmeldungDurchAnderesOrchestermitglied { get; private set; }
        public bool IstAnwesend { get; private set; } = false;
        public string? KommentarAnwesenheit { get; private set; }

        private TerminRückmeldungOrchestermitglied() { }

        private TerminRückmeldungOrchestermitglied(RückgemeldetePersonId id, OrchesterMitgliedsId orchesterMitgliedsId, List<Instrument> instruments, List<Notenstimme> notenstimmen): base(id)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
            _instruments = instruments;
            _notenstimmen = notenstimmen;
        }

        public static TerminRückmeldungOrchestermitglied Create(OrchesterMitgliedsId orchesterMitgliedsId, List<Instrument> defaultInstruments, List<Notenstimme> defaultNotenstimmen)
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

        public void ChangeInstruments(List<Instrument> instruments)
        {
            _instruments = instruments;
        }

        public void ChangeNotenstimme(List<Notenstimme> notenstimmen)
        {
            _notenstimmen = notenstimmen;
        }
    }
}
