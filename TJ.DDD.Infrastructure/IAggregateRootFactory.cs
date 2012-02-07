using System;

namespace TJ.DDD.Infrastructure
{
    public interface IAggregateRootFactory
    {
        T Load<T>(Guid aggregateId) where T : AggregateRoot, new();
    }
}