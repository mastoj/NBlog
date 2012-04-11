using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Event;
using TJ.Extensions;

namespace NBlog.Views
{
    public class PostView
    {
        private readonly IPostViewRepostiory _postViewRepostiory;

        public PostView(IPostViewRepostiory postViewRepostiory)
        {
            _postViewRepostiory = postViewRepostiory;
        }

        public void Handle(PostCreatedEvent postCreatedEvent)
        {
            var postItem = new PostItem()
            {
                Title = postCreatedEvent.Title,
                Slug = postCreatedEvent.Slug,
                Tags = postCreatedEvent.Tags,
                Content = postCreatedEvent.Content,
                Excerpt = postCreatedEvent.Excerpt,
                CreationDate = postCreatedEvent.CreationDate,
                PostId = postCreatedEvent.AggregateId
            };
            _postViewRepostiory.Insert(postItem);
        }

        public IEnumerable<PostItem> GetPosts(bool includeDeletedPosts = false)
        {
            return _postViewRepostiory.All(y => y.IsDeleted.IsFalse() || includeDeletedPosts);
        }

        public void Handle(PostPublishedEvent postPublishedEvent)
        {
            var post = _postViewRepostiory.Find(y => y.PostId == postPublishedEvent.AggregateId);
            if (post.IsNotNull())
            {
                post.PublishedTime = postPublishedEvent.PublishTime;
                post.IsPublished = true;
            }
        }

        public IEnumerable<PostItem> GetPublishedPosts()
        {
            return _postViewRepostiory.All(y => y.IsPublished && y.IsDeleted.IsFalse());
        }

        public void Handle(PostDeletedEvent postPublishedEvent)
        {
            var post = _postViewRepostiory.Find(y => y.PostId == postPublishedEvent.AggregateId);
            if (post.IsNotNull())
            {
                post.IsDeleted = true;
            }
        }
    }

    public interface IPostViewRepostiory
    {
        void Insert(PostItem postItem);
        PostItem Find(Func<PostItem, bool> func);
        IEnumerable<PostItem> All(Func<PostItem, bool> func);
    }
}
