using System;

namespace TJ.DDD.Infrastructure
{
    public interface IDomainEvent
    {
        DateTime TimeStamp { get; set; }
        Guid AggregateId { get; set; }
    }
}