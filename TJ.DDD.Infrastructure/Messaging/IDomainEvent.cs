using System;

namespace TJ.DDD.Infrastructure.Messaging
{
    public interface IDomainEvent : IMessage
    {
        Guid Id { get; }
        DateTime TimeStamp { get; }
        Guid AggregateId { get; set; }
        int EventNumber { get; set; }
    }
}