using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Data.Mongo.Tests.EventRepository;
using TJ.Extensions;

namespace TJ.DDD.Infrastructure
{
    public abstract class AggregateRoot
    {
        private Dictionary<Type, Action<IDomainEvent>> _registeredEventHandlers;

        public AggregateRoot()
        {
            _registeredEventHandlers = new Dictionary<Type, Action<IDomainEvent>>();
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IDomainEvent
        {
            _registeredEventHandlers.Add(typeof(TEvent), @event => eventHandler(@event as TEvent));
        }

        public void Apply(IDomainEvent @event)
        {
            var eventType = @event.GetType();
            if (_registeredEventHandlers.ContainsKey(eventType))
            {
                _registeredEventHandlers[eventType](@event);
            }
        }

        public Guid AggregateId { get; set; }
    }

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
