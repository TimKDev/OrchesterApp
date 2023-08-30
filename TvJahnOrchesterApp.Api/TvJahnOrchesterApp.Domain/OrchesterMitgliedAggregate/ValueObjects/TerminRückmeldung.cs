using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Enums;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects
{
    public sealed class TerminRückmeldung : ValueObject
    {
        public TerminId TerminId { get; private set; }
        public OrchesterMitgliedsId OrchesterMitgliedsId { get; private set; }
        public Rückmeldung Rückmeldung { get; private set; }

        private TerminRückmeldung() { }

        private TerminRückmeldung(TerminId terminId, OrchesterMitgliedsId orchesterMitgliedsId, Rückmeldung rückmeldung)
        {
            TerminId = terminId;
            OrchesterMitgliedsId = orchesterMitgliedsId;
            Rückmeldung = rückmeldung;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        public static TerminRückmeldung Create(TerminId terminId, OrchesterMitgliedsId orchesterMitgliedsId)
        {
            return new TerminRückmeldung(terminId, orchesterMitgliedsId, Rückmeldung.NichtZurückgemeldet);
        }
    }
}
