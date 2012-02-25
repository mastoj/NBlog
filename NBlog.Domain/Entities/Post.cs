using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
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
        private DateTime _publishTime;

        public Post()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<CreatePostEvent>(PostCreated);
            RegisterEventHandler<UpdatePostEvent>(PostUpdated);
            RegisterEventHandler<PublishPostEvent>(PostPublished);
        }

        private void PostPublished(PublishPostEvent postPublishedEvent)
        {
            _publishTime = postPublishedEvent.PublishTime;
            _published = true;
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

        private void PostUpdated(UpdatePostEvent postUpdatedEvent)
        {
            _title = postUpdatedEvent.Title;
            _shortUrl = postUpdatedEvent.ShortUrl;
            _tags = postUpdatedEvent.Tags;
            _excerpt = postUpdatedEvent.Excerpt;
            _content = postUpdatedEvent.Content;
        }

        private Post(string title, string content, string shortUrl, List<string> tags, string excerpt, Guid aggregateId)
            : this()
        {
            var creationDate = DateTime.Now;
            var createEvent = new CreatePostEvent(title, content, shortUrl, tags, excerpt, creationDate, aggregateId);
            Apply(createEvent);
        }

        public static Post Create(string title, string content, string shortUrl, List<string> tags, string excerpt, Guid aggregateId)
        {
            return new Post(title, content, shortUrl, tags, excerpt, aggregateId);
        }

        public void Update(string title, string content, string shortUrl, List<string> tags, string excerpt)
        {
            var lastSaveTime = DateTime.Now;
            var updateEvent = new UpdatePostEvent(title, content, shortUrl, tags, excerpt, lastSaveTime, AggregateId);
            Apply(updateEvent);
        }

        public void Publish()
        {
            if (_published)
            {
                throw new PostAlreadyPublishedException();
            }
            var publishTime = DateTime.Now;
            var publishEvent = new PublishPostEvent(publishTime, AggregateId);
            Apply(publishEvent);
        }
    }
}
