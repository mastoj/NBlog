using System;
using System.Linq;
using TJ.DDD.Infrastructure.Event;
using TJ.Extensions;

namespace TJ.DDD.Infrastructure
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly IEventStore _eventStore;

        public AggregateRootFactory(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public T Load<T>(Guid aggregateId) where T : AggregateRoot, new()
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