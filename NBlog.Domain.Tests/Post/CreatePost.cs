using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NBlog.Domain.Tests.Stubs;
using NBlog.Domain.Views;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Tests;

namespace NBlog.Domain.Tests.Post.Create
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
            createPostCommandHandler.Handle(createPostCommand);
        }

        [Test]
        public void An_Post_Already_Exist_For_Url_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostAlreadyExistsForUrlException>();
        }
    }

    [TestFixture]
    public class When_Creating_A_Post : BaseTestSetup
    {
        protected override void Given()
        {
            _lowestPossibleDate = DateTime.Now;
            _title = "Title";
            _content = "content";
            _shortUrl = "shortUrl";
            _tags = new List<string>() {"tag1", "tag2"};
            _excerpt = "excerpt";
            var createPostCommand = new CreatePostCommand(_title, _content, _shortUrl, _tags, _excerpt);
            _postRepository = new StubPostRepository();
            IPostView postView = new StubPostView();
            var createPostCommandHandler = new CreatePostCommandHandler(_postRepository, postView);
            createPostCommandHandler.Handle(createPostCommand);
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            // Assert
            _postRepository.Posts.Count.Should().Be(1);
        }

        [Test]
        public void The_Post_Should_Contain_One_Uncommited_Event()
        {
            var post = _postRepository.Posts.First();
            var changes = post.GetChanges();
            changes.Count().Should().Be(1);
            changes.First().Should().BeOfType<CreatePostEvent>();
        }

        [Test]
        public void The_Uncommited_Event_Should_Have_The_Right_Information_Set()
        {
            var createPostEvent =
            _postRepository.Posts.First().GetChanges().First() as CreatePostEvent;
            createPostEvent.Content.Should().Be(_content);
            createPostEvent.EventNumber.Should().Be(0);
            createPostEvent.Excerpt.Should().Be(_excerpt);
            createPostEvent.ShortUrl.Should().Be(_shortUrl);
            createPostEvent.Tags.SequenceEqual(_tags);
            createPostEvent.Title.Should().Be(_title);
            createPostEvent.CreationDate.Should().BeAfter(_lowestPossibleDate);
        }

        private StubPostRepository _postRepository;
        private string _title;
        private string _content;
        private string _shortUrl;
        private List<string> _tags;
        private string _excerpt;
        private DateTime _lowestPossibleDate;
    }
}
