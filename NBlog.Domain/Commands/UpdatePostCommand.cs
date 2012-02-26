using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure.Messaging;

namespace NBlog.Domain.Commands
{
    public class UpdatePostCommand : Command
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortUrl { get; set; }
        public List<string> Tags { get; set; }
        public string Excerpt { get; set; }

        public UpdatePostCommand(string title, string content, string shortUrl, List<string> tags, string excerpt, Guid aggregateId) : base(aggregateId)
        {
            Title = title;
            Content = content;
            ShortUrl = shortUrl;
            Tags = tags;
            Excerpt = excerpt;
        }
    }
}