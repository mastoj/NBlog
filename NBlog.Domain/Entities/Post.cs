using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using TJ.DDD.Infrastructure;

namespace NBlog.Domain.Entities
{
    public class Post : AggregateRoot
    {
        private string _title;
        private string _shortUrl;
        private string _excerpt;
        private List<string> _tags;
        private string _content;

        public Post()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<CreatePostEvent>(PostCreated);
        }

        private void PostCreated(CreatePostEvent postCreatedEvent)
        {
            AggregateId = postCreatedEvent.AggregateId;
            _title = postCreatedEvent.Title;
            _shortUrl = postCreatedEvent.ShortUrl;
            _tags = postCreatedEvent.Tags;
            _excerpt = postCreatedEvent.Excerpt;
            _content = postCreatedEvent.Content;
        }

        private Post(string title, string shortUrl, string content, List<string> tags, string excerpt, Guid aggregateId)
            : this()
        {
            var createEvent = new CreatePostEvent(title, content, shortUrl, tags, excerpt, aggregateId);
            Apply(createEvent);
        }

        public static Post Create(string title, string content, string shortUrl, List<string> tags, string excerpt, Guid aggregateId)
        {
            return new Post(title, content, shortUrl, tags, excerpt, aggregateId);
        }
    }
}
