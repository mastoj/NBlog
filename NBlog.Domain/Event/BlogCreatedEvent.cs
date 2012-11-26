using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UserAddedToBlogEvent : DomainEventBase
    {
        public Guid UserId { get; set; }

        public UserAddedToBlogEvent(Guid userId, Guid aggregateId)
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

    public class GoogleAnalyticsEnabledEvent : DomainEventBase
    {
        public string UAAccount { get; set; }

        public GoogleAnalyticsEnabledEvent(string uaAccount, Guid aggregateId)
        {
            UAAccount = uaAccount;
            AggregateId = aggregateId;
        }
    }

    public class DisqusEnabledEvent : DomainEventBase
    {
        public string ShortName { get; set; }

        public DisqusEnabledEvent(string shortName, Guid aggregateId)
        {
            ShortName = shortName;
            AggregateId = aggregateId;
        }
    }
    public class RedirectUrlAddedEvent : DomainEventBase
    {
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }

        public RedirectUrlAddedEvent(Guid aggregateId, string oldUrl, string newUrl)
        {
            AggregateId = aggregateId;
            OldUrl = oldUrl;
            NewUrl = newUrl;
        }
    }
}