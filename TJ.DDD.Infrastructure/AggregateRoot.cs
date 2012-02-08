﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TJ.DDD.Infrastructure
{
    public abstract class AggregateRoot
    {
        private Dictionary<Type, Action<IDomainEvent>> _registeredEventHandlers;
        private List<IDomainEvent> _changes;

        public AggregateRoot()
        {
            _registeredEventHandlers = new Dictionary<Type, Action<IDomainEvent>>();
            _changes = new List<IDomainEvent>();
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> eventHandler) where TEvent : class, IDomainEvent
        {
            _registeredEventHandlers.Add(typeof(TEvent), @event => eventHandler(@event as TEvent));
        }

        protected void Apply<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            var eventType = typeof(TEvent);
            @event.SetAggregateId(AggregateId);
            var eventNumber = Version + 1;
            @event.SetEventNumber(eventNumber);
            Version = eventNumber;
            _changes.Add(@event);
            Apply(eventType, @event);
        }

        public void LoadAggregate(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                var eventType = domainEvent.GetType();
//                Version = domainEvent.EventNumber;
                Apply(eventType, domainEvent);
            }
        }

        private void Apply(Type eventType, IDomainEvent @event)
        {
            if (_registeredEventHandlers.ContainsKey(eventType))
            {
                Action<IDomainEvent> eventHandler;
                eventHandler = _registeredEventHandlers[eventType];
                eventHandler(@event);
            }
        }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public IEnumerable<IDomainEvent> GetChanges()
        {
            return _changes;
        }
    }
}
