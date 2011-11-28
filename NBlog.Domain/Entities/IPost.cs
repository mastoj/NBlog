using System;
using System.Collections.Generic;
using NBlog.Domain;

namespace NBlog.Domain
{
    public interface IPost : IEntity
    {
        string ShortUrl { get; set; }
        string Content { get; set; }
        string Excerpt { get; set; }
        string Title { get; set; }
        bool Publish { get; set; }
        DateTime PublishDate { get; set; }
        IList<string> Tags { get; set; }
        IList<string> Categories { get; set; }
    }
}