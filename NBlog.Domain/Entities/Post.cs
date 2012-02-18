using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Event;

namespace NBlog.Domain.Entities
{
    public class Blog : AggregateRoot
    {
        private string _title;

        private Blog(string title, Guid aggregateId) : this()
        {
            var createEvent = new CreateBlogEvent(title, aggregateId);
            Apply(createEvent);
        }

        private Blog()
        {
            RegisterEventHandler<CreateBlogEvent>(BlogCreated);
        }

        private void BlogCreated(CreateBlogEvent createBlogEvent)
        {
            _title = createBlogEvent.Title;
            AggregateId = createBlogEvent.AggregateId;
        }

        public static Blog Create(string title, Guid aggregateId)
        {
            return new Blog(title, aggregateId);
        }
    }

    public class CreateBlogEvent : DomainEventBase
    {
        public string Title { get; set; }

        public CreateBlogEvent(string title, Guid aggregateId)
        {
            Title = title;
            AggregateId = aggregateId;
        }
    }

    public class Post : AggregateRoot
    {
        private string _title;
        private string _shortUrl;
        private string _excerpt;
        private List<string> _tags;
        private string _content;
        private bool _published;

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
            _published = false;
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
