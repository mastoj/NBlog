using System;

namespace TJ.DDD.Infrastructure
{
    public interface IAggregateRespository<T> where T : AggregateRoot, new()
    {
        T Get<T>(Guid aggregateId);
    }
}