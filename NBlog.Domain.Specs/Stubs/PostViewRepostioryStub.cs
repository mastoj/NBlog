using System;
using System.Collections.Generic;
using System.Linq;
using NBlog.Views;

namespace NBlog.Domain.Specs.Stubs
{
    public class PostViewRepostioryStub : IPostViewRepostiory
    {
        private List<PostItem> _posts;

        public PostViewRepostioryStub()
        {
            _posts = new List<PostItem>();
        }

        public void Insert(PostItem postItem)
        {
            _posts.Add(postItem);
        }

        public PostItem Find(Func<PostItem, bool> func)
        {
            return _posts.FirstOrDefault(func);
        }

        public IEnumerable<PostItem> All(Func<PostItem, bool> func)
        {
            return _posts.Where(func);
        }
    }
}