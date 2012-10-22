using System;
using System.Collections.Generic;
using NBlog.Domain.Entities;

namespace NBlog.Views
{
    public class PostItem : IPostData
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string HtmlContent { get; set; }
        public string Excerpt { get; set; }
        public string HtmlExcerpt { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid AggregateId { get; set; }
        public DateTime PublishedTime { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastSaveTime { get; set; }
    }
}