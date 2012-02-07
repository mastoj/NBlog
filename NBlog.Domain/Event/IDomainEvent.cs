using System;

namespace NBlog.Domain.Event
{
    public interface IDomainEvent
    {
        DateTime TimeStamp { get; set; }
        Guid AggregateId { get; set; }
    }
}