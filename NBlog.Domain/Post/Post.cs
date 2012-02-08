using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.MongoEvent;

namespace NBlog.Domain.Post
{

    public class Post : AggregateRoot
    {
        private string _title;
        private string _shortUrl;

        public Post()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<PostCreatedEvent>(PostCreated);
        }

        private void PostCreated(PostCreatedEvent postCreatedEvent)
        {
            _title = postCreatedEvent.Title;
            _shortUrl = postCreatedEvent.ShortUrl;
        }

        private Post(string title, string shortUrl)
            : this()
        {
            var createEvent = new PostCreatedEvent(title, shortUrl);
            Apply(createEvent);
        }

        public static Post Create(string title, string shortUrl)
        {
            return new Post(title, shortUrl);
        }
    }

    public class PostCreatedEvent : DomainEventBase
    {
        public string Title { get; private set; }
        public string ShortUrl { get; private set; }

        public PostCreatedEvent(string title, string shortUrl)
        {
            Title = title;
            ShortUrl = shortUrl;
        }
    }
}
