using System;
using System.Collections.Generic;
using NBlog.Data;

namespace NBlog.Data
{
    public interface IPost : IEntity
    {
        string ShortUrl { get; set; }
        string Content { get; set; }
        string Title { get; set; }
        bool Publish { get; set; }
        DateTime? PublishDate { get; set; }
        IList<string> Tags { get; set; }
        IList<string> Categories { get; set; }
    }
}