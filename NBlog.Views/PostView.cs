using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownSharp;
using NBlog.Domain.Event;
using TJ.Extensions;

namespace NBlog.Views
{
    public interface IPostView
    {
        IEnumerable<PostItem> GetPublishedPosts();
        IEnumerable<PostItem> GetPosts(bool includeDeletedPosts = false);
        void Handle(PostCreatedEvent postCreatedEvent);
        void Handle(PostPublishedEvent postPublishedEvent);
        void Handle(PostUnpublishedEvent postPublishedEvent);
        void Handle(PostDeletedEvent postPublishedEvent);
        void Handle(PostUpdatedEvent postUpdatedEvent);
        PostItem GetPostWithSlug(string slug);
    }

    public class PostView : IPostView
    {
        private readonly IViewRepository<PostItem> _postViewRepostiory;
        private Markdown _markdown;

        public PostView(IViewRepository<PostItem> postViewRepostiory)
        {
            _postViewRepostiory = postViewRepostiory;
            _markdown = new Markdown();
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
                HtmlContent = Transform(postCreatedEvent.Content),
                HtmlExcerpt = Transform(postCreatedEvent.Excerpt),
                CreationDate = postCreatedEvent.CreationDate,
                AggregateId = postCreatedEvent.AggregateId,
                LastSaveTime = postCreatedEvent.CreationDate
            };
            _postViewRepostiory.Insert(postItem);
        }

        public IEnumerable<PostItem> GetPosts(bool includeDeletedPosts = false)
        {
            return _postViewRepostiory.All(y => y.IsDeleted.IsFalse() || includeDeletedPosts);
        }

        public IEnumerable<PostItem> GetPublishedPosts()
        {
            return _postViewRepostiory.All(y => y.IsPublished && y.IsDeleted.IsFalse());
        }

        public void Handle(PostPublishedEvent postPublishedEvent)
        {
            var post = _postViewRepostiory.Find(y => y.AggregateId == postPublishedEvent.AggregateId);
            if (post.IsNotNull())
            {
                post.PublishedTime = postPublishedEvent.PublishTime;
                post.IsPublished = true;
            }
        }

        public void Handle(PostDeletedEvent postDeletedEvent)
        {
            var post = _postViewRepostiory.Find(y => y.AggregateId == postDeletedEvent.AggregateId);
            if (post.IsNotNull())
            {
                post.IsDeleted = true;
            }
        }

        public void Handle(PostUpdatedEvent postUpdatedEvent)
        {
            var post = _postViewRepostiory.Find(y => y.AggregateId == postUpdatedEvent.AggregateId);
            if (post != null)
            {
                post.LastSaveTime = postUpdatedEvent.LastSaveTime;
                post.Content = postUpdatedEvent.Content;
                post.Excerpt = postUpdatedEvent.Excerpt;
                post.HtmlContent = Transform(postUpdatedEvent.Content);
                post.HtmlExcerpt = Transform(postUpdatedEvent.Excerpt);
                post.Slug = postUpdatedEvent.Slug;
                post.Tags = postUpdatedEvent.Tags;
                post.Title = postUpdatedEvent.Title;
            }
        }

        public PostItem GetPostWithSlug(string slug)
        {
            return _postViewRepostiory.Find(y => y.Slug == slug);
        }

        private string Transform(string markdownText)
        {
            return _markdown.Transform(markdownText);
        }


        public void Handle(PostUnpublishedEvent postUnpublishedEvent)
        {
            var post = _postViewRepostiory.Find(y => y.AggregateId == postUnpublishedEvent.AggregateId);
            if (post.IsNotNull())
            {
                post.IsPublished = false;
            }
        }
    }
}
