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

        public IEnumerable<TType> ResolveAll<TType>()
        {
            
        }

        public DomainEventManager(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var handlers = _eventHandlerFactory.GetHandlers<TEvent>();
            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
        }
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

    public interface IDomainEvent
    {
    }
}
