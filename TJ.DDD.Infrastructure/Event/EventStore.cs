using System;
using System.Collections.Generic;
using System.Linq;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.Infrastructure.Messaging;

namespace TJ.DDD.Infrastructure.Event
{
    public abstract class EventStore : IEventStore, IUnitOfWork
    {
        private readonly IPublishEvent _eventPublisher;
        private Dictionary<Guid, AggregateRoot> _aggregateDictionary;

        protected abstract void InsertBatch(IEnumerable<IDomainEvent> eventBatch);
        protected abstract IEnumerable<IDomainEvent> GetEvents(Guid aggregateId);

        public EventStore(IPublishEvent eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _aggregateDictionary = new Dictionary<Guid, AggregateRoot>();
        }

        public T Get<T>(Guid aggregateId) where T : AggregateRoot, new()
        {
            if (_aggregateDictionary.ContainsKey(aggregateId))
            {
                return _aggregateDictionary[aggregateId] as T;
            }
            var events = GetEvents(aggregateId).ToList();
            if (events.Count == 0)
            {
                throw new ArgumentException("No aggregate with id " + aggregateId.ToString(), "aggregateId");
            }
            T aggregate = new T();
            aggregate.LoadAggregate(events);
            _aggregateDictionary.Add(aggregateId, aggregate);
            return aggregate;
        }

        public void Insert<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            _aggregateDictionary.Add(aggregate.AggregateId, aggregate);
        }

        public void Rollback()
        {
            ClearEvents();
        }

        public void Commit()
        {
            var uncommitedEvents = GetUncommitedEvents();
            InsertBatch(uncommitedEvents);
            foreach (var uncommitedEvent in uncommitedEvents)
            {
                _eventPublisher.Publish(uncommitedEvent);
            }
            ClearEvents();
        }

        private void ClearEvents()
        {
            foreach (var aggregateRoot in _aggregateDictionary)
            {
                aggregateRoot.Value.ClearChanges();
            }
            _aggregateDictionary = new Dictionary<Guid, AggregateRoot>();
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