using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Models;

namespace TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects
{
    public sealed class OrchesterMitgliedsId : AggregateRootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private OrchesterMitgliedsId() { }

        private OrchesterMitgliedsId(Guid value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static OrchesterMitgliedsId CreateUnique()
        {
            return new OrchesterMitgliedsId { Value = Guid.NewGuid() };
        }

        public static OrchesterMitgliedsId Create(Guid value)
        {
            return new OrchesterMitgliedsId(value);
        }
    }
}
