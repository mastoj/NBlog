using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBlog.Data.DTO
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
