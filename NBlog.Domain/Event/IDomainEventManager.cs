using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBlog.Domain.Event
{
    public class DomainEventManager : IDomainEventManager
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IEventStoreFactory _eventStoreFactory;

        public DomainEventManager(IEventHandlerFactory eventHandlerFactory, IEventStoreFactory eventStoreFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _eventStoreFactory = eventStoreFactory;
        }

        public void RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var handlers = _eventHandlerFactory.GetHandlers<TEvent>();

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
            var eventStore = _eventStoreFactory.GetEventStore<TEvent>();
            eventStore.SaveEvent(@event);
        }
    }

    public interface IEventStoreFactory
    {
        IEventStore<T> GetEventStore<T>() where T : class, IDomainEvent;
    }

    public interface IEventStore<T> where T : class, IDomainEvent
    {
        void SaveEvent(T @event);
        IEnumerable<T> GetEvents();
    }

    public class EventHandlerFactory : IEventHandlerFactory
    {
        private Dictionary<Type, IEnumerable<IHandle<object>>> _handlerDictionary;
        private IEnumerable<IHandle<object>> _emptyList;

        public EventHandlerFactory()
        {
            _handlerDictionary = new Dictionary<Type, IEnumerable<IHandle<object>>>();
        }

        public IEnumerable<IHandle<object>> GetHandlers<TEvent>() where TEvent : class, IDomainEvent
        {
            var type = typeof (TEvent);
            if (_handlerDictionary.ContainsKey(type))
            {
                return _handlerDictionary[type];
            }
            return _emptyList;
        }
    }

    public interface IEventHandlerFactory
    {
        IEnumerable<IHandle<object>> GetHandlers<TEvent>() where TEvent : class, IDomainEvent;
    }

    public interface IDomainEventManager
    {
        void RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
    }

    public interface IDomainEvent : IEntity
    {
    }

    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime TimeStamp { get; set; }

        public DomainEventBase()
        {
            TimeStamp = DateTime.UtcNow;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (DomainEventBase)) return false;
            return Equals((DomainEventBase) obj);
        }

        public bool Equals(DomainEventBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode();
            }
        }
    }
}
