using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class PostPublishedEvent : DomainEventBase
    {
        public DateTime PublishTime { get; set; }

        public PostPublishedEvent(DateTime publishTime, Guid aggregateId)
        {
            PublishTime = publishTime;
            AggregateId = aggregateId;
        }
    }
}