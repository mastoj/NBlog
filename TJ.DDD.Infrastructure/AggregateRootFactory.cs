using System;
using System.Linq;
using NBlog.Data.Mongo.Tests.EventRepository;
using TJ.Extensions;

namespace TJ.DDD.Infrastructure
{
    public class AggregateRootFactory
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
                foreach (var domainEvent in events)
                {
                    aggregate.Apply(domainEvent);
                }
                return aggregate;
            }
            throw new ArgumentException("No aggregate with id: " + aggregateId, "aggregateId");
        }
    }
}