using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects
{
    public sealed class ZeitblockId: ValueObject
    {
        public Guid Value { get; private set; }

        private ZeitblockId() { }

        private ZeitblockId(Guid value)
        {
            Value = value;
        }

        public static ZeitblockId CreateUnique()
        {
            return new ZeitblockId(Guid.NewGuid());
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
