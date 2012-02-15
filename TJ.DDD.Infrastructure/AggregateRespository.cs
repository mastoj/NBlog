using System;
using System.Linq;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.Infrastructure.Event;
using TJ.Extensions;

namespace TJ.DDD.Infrastructure
{
    public class AggregateRespository<T> //: IAggregateRespository<T> 
        where T : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;

        public AggregateRespository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public T Get(Guid aggregateId)
        {
            var events = _eventStore.GetEvents(aggregateId).ToList();
            if (events.IsNotNull() && events.Count > 0)
            {
                T aggregate = new T();
                aggregate.LoadAggregate(events);
                return aggregate;
            }
            throw new ArgumentException("No aggregate with id: " + aggregateId, "aggregateId");
        }
    }
}