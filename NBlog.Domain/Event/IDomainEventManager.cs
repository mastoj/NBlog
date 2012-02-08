using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Event;

namespace NBlog.Domain.Event
{
//    public class DomainEventManager : IDomainEventManager
//    {
//        private readonly IEventHandlerFactory _eventHandlerFactory;
//        private readonly IEventStoreFactory _eventStoreFactory;

//        public DomainEventManager(IEventHandlerFactory eventHandlerFactory, IEventStoreFactory eventStoreFactory)
//        {
//            _eventHandlerFactory = eventHandlerFactory;
//            _eventStoreFactory = eventStoreFactory;
//        }

//        public void RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
//        {
//            var handlers = _eventHandlerFactory.GetHandlers<TEvent>();

//            foreach (var handler in handlers)
//            {
//                handler.Handle(@event);
//            }
//            var eventStore = _eventStoreFactory.GetEventStore<TEvent>();
//            eventStore.SaveEvent(@event);
//        }
//    }

//    public interface IEventStoreFactory
//    {
//        IEventStore<T> GetEventStore<T>() where T : class, IDomainEvent;
//    }

//    public class EventHandlerFactory : IEventHandlerFactory
//    {
//        private Dictionary<Type, IEnumerable<IHandle<object>>> _handlerDictionary;
//        private IEnumerable<IHandle<object>> _emptyList;

//        public EventHandlerFactory()
//        {
//            _handlerDictionary = new Dictionary<Type, IEnumerable<IHandle<object>>>();
//        }

//        public IEnumerable<IHandle<object>> GetHandlers<TEvent>() where TEvent : class, IDomainEvent
//        {
//            var type = typeof (TEvent);
//            if (_handlerDictionary.ContainsKey(type))
//            {
//                return _handlerDictionary[type];
//            }
//            return _emptyList;
//        }
//    }

//    public interface IEventHandlerFactory
//    {
//        IEnumerable<IHandle<object>> GetHandlers<TEvent>() where TEvent : class, IDomainEvent;
//    }

    public interface IDomainEventManager
    {
        void RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
    }
}
