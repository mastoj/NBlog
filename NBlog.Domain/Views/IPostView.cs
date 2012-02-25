using System.Collections.Generic;

namespace NBlog.Domain.Views
{
    public interface IPostView
    {
        IEnumerable<PostViewItem> Get();
        void Insert(PostViewItem postViewItem);
    }
}
