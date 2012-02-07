using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure;

namespace NBlog.Data.Mongo.Tests.EventRepository
{
    public interface IEventStore
    {
        void Insert(IDomainEvent domainEvent);
        IEnumerable<IDomainEvent> GetEvents(Guid aggregateId);
    }
}