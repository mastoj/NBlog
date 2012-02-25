using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure.Event;

namespace NBlog.Domain.Event
{
    public class UpdatePostEvent : DomainEventBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortUrl { get; set; }

        public List<string> Tags { get; set; }

        public string Excerpt { get; set; }

        public DateTime LastSaveTime { get; set; }

        public UpdatePostEvent(string title, string content, string shortUrl, List<string> tags, string excerpt, DateTime lastSaveTime, Guid aggregateId)
        {
            Title = title;
            Content = content;
            ShortUrl = shortUrl;
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