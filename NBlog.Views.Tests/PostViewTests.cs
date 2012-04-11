﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.Event;
using NUnit.Framework;
using TJ.Extensions;

namespace NBlog.Views.Tests
{
    [TestFixture]
    public class When_Post_Created_To_Empty_List : PostViewTestBase
    {
        public override void When()
        {
            _postCreatedEvent = new PostCreatedEvent("Title", "Content", "slug", new List<string>() {"tag1", "tag2"}, "Excerpt", DateTime.Now, Guid.NewGuid());
            PostView.Handle(_postCreatedEvent);
        }

        [Test]
        public void It_Should_Exist_One_Post()
        {
            var posts = PostView.GetPosts();
            posts.Count().Should().Be(1);
        }

        [Test]
        public void And_The_Post_Should_Have_The_Right_Data()
        {
            var post = PostView.GetPosts().First();
            post.Content.Should().Be(_postCreatedEvent.Content);
            post.CreationDate.Should().Be(_postCreatedEvent.CreationDate);
            post.Excerpt.Should().Be(_postCreatedEvent.Excerpt);
            post.PostId.Should().Be(_postCreatedEvent.AggregateId);
            post.Slug.Should().Be(_postCreatedEvent.Slug);
            post.Title.Should().Be(_postCreatedEvent.Title);
            post.Tags.Should().Contain(_postCreatedEvent.Tags);
            post.IsPublished.Should().BeFalse();
        }

        private PostCreatedEvent _postCreatedEvent;
    }

    [TestFixture]
    public class When_A_Post_Is_Published : PostViewTestBase
    {
        public override void Given()
        {
            _postId = Guid.NewGuid();
            var postCreatedEvent = new PostCreatedEvent("Title", "Content", "slug", new List<string>() { "tag1", "tag2" }, "Excerpt", DateTime.Now.AddDays(-1), _postId);
            PostView.Handle(postCreatedEvent);
            AddXPostItemsToView(5, PostView);
        }

        public override void When()
        {
            _postPublihsedEvent = new PostPublishedEvent(DateTime.Now, _postId);
            PostView.Handle(_postPublihsedEvent);
        }

        
        [Test]
        public void It_Should_Be_One_Published_Post()
        {
            var posts = PostView.GetPublishedPosts();
            posts.Count().Should().Be(1);
        }

        [Test]
        public void It_Should_Have_Correct_Post_Id()
        {
            var post = PostView.GetPublishedPosts().Last();
            post.PostId.Should().Be(_postPublihsedEvent.AggregateId);
        }

        [Test]
        public void It_Should_Have_Correct_Published_Date()
        {
            var post = PostView.GetPublishedPosts().LastOrDefault();
            post.PublishedTime.Should().Be(_postPublihsedEvent.PublishTime);
        }

        private PostPublishedEvent _postPublihsedEvent;
        private Guid _postId;
    }

    [TestFixture]
    public class When_Unpublished_Post_Is_Deleted : PostViewTestBase
    {
        public override void Given()
        {
            _postId = Guid.NewGuid();
            var postCreatedEvent = new PostCreatedEvent("Title", "Content", "slug", new List<string>() { "tag1", "tag2" }, "Excerpt", DateTime.Now.AddDays(-1), _postId);
            PostView.Handle(postCreatedEvent);
            AddXPostItemsToView(5, PostView);
            _initialPostCount = PostView.GetPosts().Count();
        }

        public override void When()
        {
            _postDeletedEvent = new PostDeletedEvent(_postId);
            PostView.Handle(_postDeletedEvent);
        }

        [Test]
        public void Then_Post_Count_Should_Decrease_By_One()
        {
            PostView.GetPosts().Count().Should().Be(_initialPostCount - 1);
        }

        [Test]
        public void The_Posts_Should_Not_Contain_Deleted_Post()
        {
            PostView.GetPosts().Any(y => y.PostId == _postId).Should().BeFalse();
        }

        private Guid _postId;
        private PostDeletedEvent _postDeletedEvent;
        private int _initialPostCount;
    }

    [TestFixture]
    public class When_Published_Post_Is_Deleted : PostViewTestBase
    {
        public override void Given()
        {
            _postId = Guid.NewGuid();
            var postCreatedEvent = new PostCreatedEvent("Title", "Content", "slug", new List<string>() { "tag1", "tag2" }, "Excerpt", DateTime.Now.AddDays(-1), _postId);
            PostView.Handle(postCreatedEvent);
            PostView.Handle(new PostPublishedEvent(DateTime.Now, _postId));
            AddXPostItemsToView(5, PostView);
            _initialPostCount = PostView.GetPosts().Count();
        }

        public override void When()
        {
            _postDeletedEvent = new PostDeletedEvent(_postId);
            PostView.Handle(_postDeletedEvent);
        }

        [Test]
        public void Then_Post_Count_Should_Decrease_By_One()
        {
            PostView.GetPosts().Count().Should().Be(_initialPostCount - 1);
        }

        [Test]
        public void Then_Posts_Should_Not_Contain_Deleted_Post()
        {
            PostView.GetPosts().Any(y => y.PostId == _postId).Should().BeFalse();
        }

        [Test]
        public void Then_Published_Post_Count_Should_Decrease_By_One()
        {
            PostView.GetPosts().Count().Should().Be(_initialPostCount - 1);
        }

        [Test]
        public void Then_Published_Posts_Should_Not_Contain_Deleted_Post()
        {
            PostView.GetPublishedPosts().Any(y => y.PostId == _postId).Should().BeFalse();
        }

        [Test]
        public void Then_Posts_Should_Included_Deleted_Post_If_Asked_For_It()
        {
            PostView.GetPosts(includeDeletedPosts: true).Any(y => y.PostId == _postId).Should().BeTrue();
            PostView.GetPosts(includeDeletedPosts: true).Count().Should().Be(_initialPostCount);
        }

        private Guid _postId;
        private PostDeletedEvent _postDeletedEvent;
        private int _initialPostCount;
    }

    [TestFixture]
    public abstract class PostViewTestBase
    {
        public PostView PostView { get; private set; }

        public PostViewTestBase()
        {
            IPostViewRepostiory postViewRepostioryStub = new PostViewRepositoryStub();
            PostView = new PostView(postViewRepostioryStub);
        }

        public virtual void Given()
        {
        }

        public abstract void When();

        [TestFixtureSetUp]
        public void SetUp()
        {
            Given();
            When();
        }

        protected void AddXPostItemsToView(int numberOfItems, PostView postView)
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                var postCreatedEvent = new PostCreatedEvent("Title", "Content", "slug",
                                                            new List<string>() {"tag1", "tag2"}, "Excerpt",
                                                            DateTime.Now.AddDays(-1), Guid.NewGuid());
                postView.Handle(postCreatedEvent);
            }
        }
    }

    public class PostViewRepositoryStub : IPostViewRepostiory
    {
        private List<PostItem> _posts;

        public PostViewRepositoryStub()
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
