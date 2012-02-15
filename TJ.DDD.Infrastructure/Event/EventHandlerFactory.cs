using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.DDD.Infrastructure.Event
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private Dictionary<Type, List<IHandle<IDomainEvent>>> _eventHandlers;

        public EventHandlerFactory()
        {
            _eventHandlers = new Dictionary<Type, List<IHandle<IDomainEvent>>>();
        }

        public void RegisterEventHandler<TEvent>(IHandle<IDomainEvent> eventHandler) where TEvent : IDomainEvent
        {
            var typeOfEvent = typeof(TEvent);
            if (!_eventHandlers.ContainsKey(typeOfEvent))
            {
                _eventHandlers.Add(typeOfEvent, new List<IHandle<IDomainEvent>>());
            }
            _eventHandlers[typeOfEvent].Add(eventHandler);
        }

        public IEnumerable<IHandle<IDomainEvent>> GetEventHandlers<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            var typeOfEvent = typeof(TEvent);
            if (_eventHandlers.ContainsKey(typeOfEvent))
            {
                return _eventHandlers[typeOfEvent];
            }
            return new List<IHandle<IDomainEvent>>();
        }
    }
}
