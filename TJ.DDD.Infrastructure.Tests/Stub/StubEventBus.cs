using System.Collections.Generic;
using TJ.DDD.Infrastructure.Messaging;

namespace TJ.DDD.Infrastructure.Tests.Stub
{
    public class StubEventBus : IBus
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

        public void Send<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            throw new System.NotImplementedException();
        }

        public event CommitMessageHandler Commit;
    }
}