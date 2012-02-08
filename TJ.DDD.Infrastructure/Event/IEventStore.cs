using System;
using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Event
{
    public interface IEventStore
    {
        void Insert(IDomainEvent domainEvent);
        IEnumerable<IDomainEvent> GetEvents(Guid aggregateId);
    }
}