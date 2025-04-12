using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchesterApp.Domain.Common.Models
{
    public abstract class AggregateRoot<TId, TIdType> : Entity<TId> where TId : AggregateRootId<TIdType>
    {
        protected AggregateRoot(TId id) : base(id)
        {
        }

        protected AggregateRoot() { }

    }
}
