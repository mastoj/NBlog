using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class PostCreatedEvent : DomainEventBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        public List<string> Tags { get; set; }

        public string Excerpt { get; set; }

        public DateTime CreationDate { get; set; }

        public PostCreatedEvent(string title, string content, string slug, List<string> tags, string excerpt, DateTime creationDate, Guid aggregateId)
        {
            Title = title;
            Content = content;
            Slug = slug;
            Tags = tags;
            Excerpt = excerpt;
            CreationDate = creationDate;
            AggregateId = aggregateId;
        }
    }
}
