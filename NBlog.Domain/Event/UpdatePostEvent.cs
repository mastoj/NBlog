using System;
using System.Collections.Generic;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UpdatePostEvent : DomainEventBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        public List<string> Tags { get; set; }

        public string Excerpt { get; set; }

        public DateTime LastSaveTime { get; set; }

        public UpdatePostEvent(string title, string content, string slug, List<string> tags, string excerpt, DateTime lastSaveTime, Guid aggregateId)
        {
            Title = title;
            Content = content;
            Slug = slug;
            Tags = tags;
            Excerpt = excerpt;
            LastSaveTime = lastSaveTime;
            AggregateId = aggregateId;
        }
    }
    public class PublishPostEvent : DomainEventBase
    {
        public DateTime PublishTime { get; set; }

        public PublishPostEvent(DateTime publishTime, Guid aggregateId)
        {
            PublishTime = publishTime;
            AggregateId = aggregateId;
        }
    }
}