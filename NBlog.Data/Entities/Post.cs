using System;
using System.Collections.Generic;

namespace NBlog.Domain.Entities
{
    public class Post : Entity
    {
        public string ShortUrl { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public bool Publish { get; set; }
        public DateTime? PublishDate { get; set; }
        public IList<string> Tags { get; set; }
        public IList<string> Categories { get; set; }
    }
}
