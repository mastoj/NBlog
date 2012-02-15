using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Event
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;

        public EventBus(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                foreach (var eventListener in _eventHandlerFactory.GetEventHandlers(domainEvent))
                {
                    eventListener.Execute(domainEvent);
                }
            }
        }
    }
}