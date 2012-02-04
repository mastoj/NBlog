using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using NBlog.Domain.Mongo;
using NBlog.Domain.Repositories;

namespace NBlog.Data.Mongo.Event
{
    public class EventStore<T> : IEventStore<T> where T : class, IDomainEvent
    {
        private IRepository<T> _eventRepository;

        public EventStore(IRepository<T> eventRespository = null)
        {
            _eventRepository = eventRespository ?? new Repository<T>();
        }

        public void SaveEvent(T @event)
        {
            _eventRepository.Insert(@event);
        }

        public IEnumerable<T> GetEvents()
        {
            return _eventRepository.All();
        }
    }
}