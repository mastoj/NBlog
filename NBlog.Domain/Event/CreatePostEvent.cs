using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.DDD.Infrastructure.Event;

namespace NBlog.Domain.Event
{
    public class CreatePostEvent : DomainEventBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private string _shortUrl;
        public string ShortUrl
        {
            get { return _shortUrl; }
            set { _shortUrl = value; }
        }

        private List<string> _tags;
        public List<string> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        private string _excerpt;
        public string Excerpt
        {
            get { return _excerpt; }
            set { _excerpt = value; }
        }

        public CreatePostEvent(string title, string content, string shortUrl, List<string> tags, string excerpt, Guid aggregateId)
        {
            _title = title;
            _content = content;
            _shortUrl = shortUrl;
            _tags = tags;
            _excerpt = excerpt;
            AggregateId = aggregateId;
        }
    }
}
