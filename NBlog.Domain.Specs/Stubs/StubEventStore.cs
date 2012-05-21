using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.CQRS.Event;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Specs.Stubs
{
    public class StubEventStore : EventStore
    {
        private List<IDomainEvent> _insertedEvents;

        public StubEventStore(IEventBus bus)
            : base(bus)
        {
            _insertedEvents = new List<IDomainEvent>();
        }

        public IEnumerable<IDomainEvent> InsertedEvents
        {
            get { return _insertedEvents; }
        }

        protected override void InsertBatch(IEnumerable<IDomainEvent> eventBatch)
        {
            _insertedEvents.AddRange(eventBatch);
        }

        protected override IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            return _insertedEvents.Where(y => y.AggregateId == aggregateId);
        }

        public void InsertEvents(IEnumerable<IDomainEvent> eventBatch)
        {
            _insertedEvents.AddRange(eventBatch);
        }
    }
}
