using System;
using System.Collections.Generic;

namespace NBlog.Domain.Views
{
    public class PostViewItem
    {
        public Guid PostId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
