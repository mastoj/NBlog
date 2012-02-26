using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Messaging;

namespace TJ.DDD.Infrastructure.Tests.Stub
{
    public class StubEventStore : EventStore
    {
        private IEnumerable<IDomainEvent> _insertedEvents;
        private Dictionary<Guid, IEnumerable<IDomainEvent>> _aggregateEventDictionary;

        public StubEventStore(IBus bus) : base(bus)
        {
            _aggregateEventDictionary = new Dictionary<Guid, IEnumerable<IDomainEvent>>();
        }

        public IEnumerable<IDomainEvent> InsertedEvents
        {
            get { return _insertedEvents; }
        }

        protected override void InsertBatch(IEnumerable<IDomainEvent> eventBatch)
        {
            _insertedEvents = eventBatch;
        }

        protected override IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            if (_aggregateEventDictionary.ContainsKey(aggregateId))
            {
                return _aggregateEventDictionary[aggregateId];
            }
            return new List<IDomainEvent>();
        }

        public void AddEventsForAggregate(Guid aggregateId, IEnumerable<IDomainEvent> events)
        {
            _aggregateEventDictionary.Add(aggregateId, events);
        }
    }
}