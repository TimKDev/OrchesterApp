using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class AnwesenheitsListenEintragId : ValueObject
    {
        public Guid Value { get; private set; }

        private AnwesenheitsListenEintragId() { }

        private AnwesenheitsListenEintragId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static AnwesenheitsListenEintragId CreateUnique()
        {
            return new AnwesenheitsListenEintragId { Value = Guid.NewGuid() };
        }
    }
}
