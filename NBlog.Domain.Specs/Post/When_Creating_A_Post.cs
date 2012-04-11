using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NBlog.Domain.Repositories;
using NBlog.Domain.Views;
using NUnit.Framework;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.Infrastructure.Respositories;
using TJ.DDD.Infrastructure.Tests;
using TJ.DDD.MongoEvent;
using TJ.Extensions;

namespace NBlog.Domain.Tests
{
    [TestFixture]
    public class When_Creating_A_Post_With_Existing_Shorturl : BaseTestSetup
    {
        protected override void Given()
        {
            var title = "Title";
            var content = "content";
            var shortUrl = "shortUrl";
            var tags = new List<string>() { "tag1", "tag2" };
            var excerpt = "excerpt";
            var createPostCommand = new CreatePostCommand(title, content, shortUrl, tags, excerpt);
            var postRepository = new StubPostRepository();
            IPostView postView = new StubPostView();
            postView.Insert(new PostViewItem() {ShortUrl = shortUrl, PostId = Guid.NewGuid()});
            var createPostCommandHandler = new CreatePostCommandHandler(postRepository, postView);
            createPostCommandHandler.Execute(createPostCommand);
        }

        [Test]
        public void An_Post_Already_Exist_For_Url_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostAlreadyExistsForUrlException>();
        }
    }

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

    [TestFixture]
    public class When_Creating_A_Post : BaseTestSetup
    {
        protected override void Given()
        {
            var title = "Title";
            var content = "content";
            var shortUrl = "shortUrl";
            var tags = new List<string>() {"tag1", "tag2"};
            var excerpt = "excerpt";
            var createPostCommand = new CreatePostCommand(title, content, shortUrl, tags, excerpt);
            _postRepository = new StubPostRepository();
            IPostView postView = new StubPostView();
            var createPostCommandHandler = new CreatePostCommandHandler(_postRepository, postView);
            createPostCommandHandler.Execute(createPostCommand);
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            // Assert
            _postRepository.Posts.Count.Should().Be(1);
        }

        private StubPostRepository _postRepository;
    }

    public class StubPostRepository : IDomainRepository<Post>
    {
        private List<Post> _posts = new List<Post>();
        public List<Post> Posts
        {
            get { return _posts; }
        }

        public void Insert(Post aggregate)
        {
            _posts.Add(aggregate);
        }
    }
}
