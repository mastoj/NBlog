using System.Collections.Generic;
using NBlog.Domain.Views;

namespace NBlog.Domain.Tests.Stubs
{
    public class StubPostView : IPostView
    {
        private List<PostViewItem> _posts;

        public StubPostView()
        {
            _posts = new List<PostViewItem>();
        }

        public void Insert(PostViewItem postViewItem)
        {
            _posts.Add(postViewItem);
        }

        public IEnumerable<PostViewItem> Get()
        {
            return _posts;
        }
    }
}