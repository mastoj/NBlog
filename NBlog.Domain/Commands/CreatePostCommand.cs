using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.DDD.Infrastructure.Messaging;

namespace NBlog.Domain.Commands
{
    public class CreatePostCommand : ICommand
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortUrl { get; set; }
        public List<string> Tags { get; set; }
        public string Excerpt { get; set; }
        private Guid _aggregateId;

        public CreatePostCommand(string title, string content, string shortUrl, List<string> tags, string excerpt)
        {
            Title = title;
            Content = content;
            ShortUrl = shortUrl;
            Tags = tags;
            Excerpt = excerpt;
            _aggregateId = Guid.NewGuid();
        }

        public Guid AggregateId
        {
            get { return _aggregateId; }
        }
    }
}
