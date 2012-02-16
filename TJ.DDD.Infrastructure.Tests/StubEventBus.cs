using System.Collections.Generic;
using TJ.DDD.Infrastructure.Event;

namespace TJ.DDD.Infrastructure.Tests
{
    public class StubEventBus : IEventBus
    {
        private IEnumerable<IDomainEvent> _publishedEvents;

        public IEnumerable<IDomainEvent> PublishedEvents
        {
            get { return _publishedEvents; }
        }

        public void Publish(IEnumerable<IDomainEvent> uncommitedEvents)
        {
            _publishedEvents = uncommitedEvents;
        }
    }
}