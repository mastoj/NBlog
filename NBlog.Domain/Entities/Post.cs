using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using TJ.CQRS;
using TJ.CQRS.Event;

namespace NBlog.Domain.Entities
{
    public class Post : AggregateRoot
    {
        private string _title;
        private string _slug;
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
            RegisterEventHandler<PostCreatedEvent>(PostCreated);
            RegisterEventHandler<PostUpdatedEvent>(PostUpdated);
            RegisterEventHandler<PostPublishedEvent>(PostPublished);
            RegisterEventHandler<PostDeletedEvent>(PostDeleted);
            RegisterEventHandler<PublishDateChangedEvent>(PostPublishDateChanged);
        }

        private void PostPublishDateChanged(PublishDateChangedEvent publishDateChangedEvent)
        {
            _publishTime = publishDateChangedEvent.PublishDate;
        }

        private void PostDeleted(PostDeletedEvent postDeletedEvent)
        {
        }

        private void PostPublished(PostPublishedEvent postPublishedEventPublishedEvent)
        {
            _publishTime = postPublishedEventPublishedEvent.PublishTime;
            _published = true;
        }

        private void PostCreated(PostCreatedEvent postCreatedEventCreatedEvent)
        {
            AggregateId = postCreatedEventCreatedEvent.AggregateId;
            _title = postCreatedEventCreatedEvent.Title;
            _slug = postCreatedEventCreatedEvent.Slug;
            _tags = postCreatedEventCreatedEvent.Tags;
            _excerpt = postCreatedEventCreatedEvent.Excerpt;
            _content = postCreatedEventCreatedEvent.Content;
            _published = false;
        }

        private void PostUpdated(PostUpdatedEvent postUpdatedEventUpdatedEvent)
        {
            _title = postUpdatedEventUpdatedEvent.Title;
            _slug = postUpdatedEventUpdatedEvent.Slug;
            _tags = postUpdatedEventUpdatedEvent.Tags;
            _excerpt = postUpdatedEventUpdatedEvent.Excerpt;
            _content = postUpdatedEventUpdatedEvent.Content;
        }

        private Post(string title, string content, string slug, List<string> tags, string excerpt, Guid aggregateId)
            : this()
        {
            var creationDate = DateTime.Now;
            var createEvent = new PostCreatedEvent(title, content, slug, tags, excerpt, creationDate, aggregateId);
            Apply(createEvent);
        }

        public static Post Create(string title, string content, string slug, List<string> tags, string excerpt, Guid aggregateId)
        {
            return new Post(title, content, slug, tags, excerpt, aggregateId);
        }

        public void Update(string title, string content, string slug, List<string> tags, string excerpt)
        {
            var lastSaveTime = DateTime.Now;
            var updateEvent = new PostUpdatedEvent(title, content, slug, tags, excerpt, lastSaveTime, AggregateId);
            Apply(updateEvent);
        }

        public void Publish()
        {
            if (_published)
            {
                throw new PostAlreadyPublishedException();
            }
            var publishTime = DateTime.Now;
            var publishEvent = new PostPublishedEvent(publishTime, AggregateId);
            Apply(publishEvent);
        }

        public void Delete()
        {
            var postDeletedEvent = new PostDeletedEvent(AggregateId);
            Apply(postDeletedEvent);
        }

        public void SetPublishDate(DateTime newPublishDate)
        {
            var publishDateChangedEvent = new PublishDateChangedEvent(AggregateId, newPublishDate);
            Apply(publishDateChangedEvent);
        }
    }
}
