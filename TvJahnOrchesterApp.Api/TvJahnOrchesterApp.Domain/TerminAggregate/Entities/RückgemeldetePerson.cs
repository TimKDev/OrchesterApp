using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.Entities
{
    public sealed class RückgemeldetePerson : Entity<RückgemeldetePersonId>
    {
        private List<Instrument> _instruments = new();
        private List<Notenstimme> _notenstimmen = new();

        public IReadOnlyList<Instrument> Instruments => _instruments.AsReadOnly();
        public IReadOnlyList<Notenstimme> Notenstimmme => _notenstimmen.AsReadOnly();
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; }
        public bool Zugesagt { get; private set; }

        private RückgemeldetePerson() { }

        private RückgemeldetePerson(RückgemeldetePersonId id, OrchesterMitgliedsId orchesterMitgliedsId, List<Instrument> instruments, List<Notenstimme> notenstimmen, bool zugesagt): base(id)
        {
            OrchesterMitgliedsId = orchesterMitgliedsId;
            Zugesagt = zugesagt;
            _instruments = instruments;
            _notenstimmen = notenstimmen;
        }

        public static RückgemeldetePerson Create(OrchesterMitgliedsId orchesterMitgliedsId, List<Instrument> defaultInstruments, List<Notenstimme> defaultNotenstimmen, bool zugesagt)
        {
            return new RückgemeldetePerson(RückgemeldetePersonId.CreateUnique(), orchesterMitgliedsId, defaultInstruments, defaultNotenstimmen, zugesagt);
        }

        public void ChangeZusage(bool zugesagt)
        {
            Zugesagt = zugesagt;
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
