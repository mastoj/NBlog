using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBlog.Domain.Entities
{
    public interface IPostData
    {
        Guid AggregateId { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        string Slug { get; set; }
        List<string> Tags { get; set; }
        string Excerpt { get; set; }
    }
}
