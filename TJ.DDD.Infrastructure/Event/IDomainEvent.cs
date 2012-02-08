using System;

namespace TJ.DDD.Infrastructure.Event
{
    public interface IDomainEvent
    {
        DateTime TimeStamp { get; }
        Guid AggregateId { get; }
        int EventNumber { get; }
        void SetEventNumber(int eventNumber);
        void SetAggregateId(Guid aggregateId);
    }
}