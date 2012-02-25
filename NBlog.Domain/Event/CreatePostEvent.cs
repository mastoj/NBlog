using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.DDD.Infrastructure.Event;

namespace NBlog.Domain.Event
{
    public class CreatePostEvent : DomainEventBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortUrl { get; set; }

        public List<string> Tags { get; set; }

        public string Excerpt { get; set; }

        public DateTime CreationDate { get; set; }

        public CreatePostEvent(string title, string content, string shortUrl, List<string> tags, string excerpt, DateTime creationDate, Guid aggregateId)
        {
            Title = title;
            Content = content;
            ShortUrl = shortUrl;
            Tags = tags;
            Excerpt = excerpt;
            CreationDate = creationDate;
            AggregateId = aggregateId;
        }
    }
}
