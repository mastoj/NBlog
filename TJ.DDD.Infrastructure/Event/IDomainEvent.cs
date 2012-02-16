using System;

namespace TJ.DDD.Infrastructure.Event
{
    public interface IDomainEvent
    {
        DateTime TimeStamp { get; }
        Guid AggregateId { get; set; }
        int EventNumber { get; set; }
    }
}