using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UserAddedToBlogEvent : DomainEventBase
    {
        public string UserId { get; set; }
        public Guid AggregateId { get; set; }

        public UserAddedToBlogEvent(string userId, Guid aggregateId)
        {
            UserId = userId;
            AggregateId = aggregateId;
        }
    }
    public class BlogCreatedEvent : DomainEventBase
    {
        public BlogCreatedEvent(string blogTitle, string subTitle, DateTime creationTime, Guid aggregateId)
        {
            BlogTitle = blogTitle;
            SubTitle = subTitle;
            CreationTime = creationTime;
            AggregateId = aggregateId;
        }

        public string BlogTitle { get; set; }
        public string SubTitle { get; set; }
        public DateTime CreationTime { get; set; }
    }
}