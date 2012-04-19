using System.Collections.Generic;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public interface IBlogView
    {
        IEnumerable<BlogViewItem> GetBlogs();
        void Handle(BlogCreatedEvent createdEvent);
    }
}