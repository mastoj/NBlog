using System;
using System.Collections.Generic;
using NBlog.Domain.Repositories;

namespace NBlog.Domain.Event
{
    public class EventStore<T> : IEventStore<T> where T : class, IDomainEvent
    {
        private readonly IRepository<T> _eventRepository;

        public EventStore(IRepository<T> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void SaveEvent(T @event)
        {
            _eventRepository.Insert(@event);
        }

        public IEnumerable<T> GetEvents(Guid aggregateId)
        {
            return _eventRepository.All();
        }
    }
}