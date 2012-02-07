using System;
using System.Collections.Generic;

namespace NBlog.Domain.Event
{
    public interface IEventStore<T> where T : class, IDomainEvent
    {
        void SaveEvent(T @event);
        IEnumerable<T> GetEvents(Guid aggregateId);
    }
}