using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class PostDeletedEvent : DomainEventBase
    {
        public PostDeletedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}