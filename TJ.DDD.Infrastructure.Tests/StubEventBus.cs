using System.Collections.Generic;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Messaging;

namespace TJ.DDD.Infrastructure.Tests
{
    public class StubEventBus : IPublishEvent
    {
        private List<IDomainEvent> _publishedEvents;

        public StubEventBus()
        {
            _publishedEvents = new List<IDomainEvent>();
        }

        public IEnumerable<IDomainEvent> PublishedEvents
        {
            get { return _publishedEvents; }
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            _publishedEvents.Add(@event);
        }
    }
}