using System;
using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Event
{
    public interface IEventStore
    {
        T Get<T>(Guid aggregateId) where T : AggregateRoot, new();
    }
}