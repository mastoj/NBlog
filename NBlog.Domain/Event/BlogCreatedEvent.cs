using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UserAddedEvent : DomainEventBase
    {
        public string AdminId { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }

        public UserAddedEvent(string adminId, string authorName, string authorEmail, Guid aggregateId)
        {
            AdminId = adminId;
            AuthorName = authorName;
            Email = authorEmail;
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