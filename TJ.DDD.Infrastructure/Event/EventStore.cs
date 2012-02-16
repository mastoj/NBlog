using System;
using System.Collections.Generic;
using System.Linq;
using TJ.DDD.Infrastructure.Command;

namespace TJ.DDD.Infrastructure.Event
{
    public abstract class EventStore : IEventStore, IUnitOfWork
    {
        private readonly IEventBus _eventBus;
        private Dictionary<Guid, AggregateRoot> _aggregateDictionary;

        protected abstract void InsertBatch(IEnumerable<IDomainEvent> eventBatch);
        protected abstract IEnumerable<IDomainEvent> GetEvents(Guid aggregateId);

        public EventStore(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _aggregateDictionary = new Dictionary<Guid, AggregateRoot>();
        }

        public T Get<T>(Guid aggregateId) where T : AggregateRoot, new()
        {
            if (_aggregateDictionary.ContainsKey(aggregateId))
            {
                return _aggregateDictionary[aggregateId] as T;
            }
            var events = GetEvents(aggregateId).ToList();
            T aggregate = new T();
            aggregate.LoadAggregate(events);
            _aggregateDictionary.Add(aggregateId, aggregate);
            return aggregate;
        }

        public void UndoChanges()
        {
            ClearEvents();
        }

        public void Commit()
        {
            var uncommitedEvents = GetUncommitedEvents();
            InsertBatch(uncommitedEvents);
            _eventBus.Publish(uncommitedEvents);
            ClearEvents();
        }

        private void ClearEvents()
        {
            foreach (var aggregateRoot in _aggregateDictionary)
            {
                aggregateRoot.Value.ClearChanges();
            }
        }

        private List<IDomainEvent> GetUncommitedEvents()
        {
            List<IDomainEvent> uncommitedEvents = new List<IDomainEvent>();
            foreach (var aggregateRoot in _aggregateDictionary)
            {
                uncommitedEvents.AddRange(aggregateRoot.Value.GetChanges());
            }
            return uncommitedEvents;
        }
    }
}