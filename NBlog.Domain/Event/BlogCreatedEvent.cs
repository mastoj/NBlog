using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UserAddedEvent : DomainEventBase
    {
        public string AdminId { get; set; }
        public string AuthorName { get; set; }

        public UserAddedEvent(string adminId, string authorName, Guid aggregateId)
        {
            AdminId = adminId;
            AuthorName = authorName;
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