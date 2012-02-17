using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.DDD.Infrastructure.Event;

namespace TJ.DDD.Infrastructure.Respositories
{
    public abstract class DomainRepository<TAggregate> : IDomainRepository<TAggregate> where TAggregate : AggregateRoot
    {
        private readonly IEventStore _eventStore;

        public DomainRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Insert(TAggregate aggregate)
        {
            _eventStore.Insert(aggregate);
        }
    }
}
