using System;
using System.Collections.Generic;
using NBlog.Domain.Entities;

namespace NBlog.Domain.Tests.Builders
{
    public class PostBuilder
    {
        private string _title;
        private string _content;
        private string _slug;
        private List<string> _tags;
        private string _excerpt;
        private Guid _aggregateId;

        public PostBuilder()
        {
            _title = "title";
            _content = "content";
            _slug = "slug";
            _tags = new List<string>() {"tag1", "tag2"};
            _excerpt = "Excerpt";
            _aggregateId = Guid.Empty;
        }

        public static implicit operator Post(PostBuilder builder)
        {
            return Post.Create(builder._title, builder._content, builder._slug, builder._tags, builder._excerpt, builder._aggregateId);
        }
    }
}