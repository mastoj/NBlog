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

    public class PostUnpublishedEvent : DomainEventBase
    {
        public PostUnpublishedEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }

    public class PublishDateChangedEvent : DomainEventBase
    {
        public DateTime PublishDate { get; set; }

        public PublishDateChangedEvent(Guid aggregateId, DateTime publishDate)
        {
            PublishDate = publishDate;
            AggregateId = aggregateId;
        }
    }
}