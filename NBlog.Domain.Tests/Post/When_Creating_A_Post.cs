using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.Post;
using NUnit.Framework;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.MongoEvent;

namespace NBlog.Domain.Tests
{
    [TestFixture]
    public class When_Creating_A_Post
    {
        private string _title;
        private string _shortUrl;
        private Domain.Post.Post _createdPost;
        private CreatePostCommand _createPostCommand;

        [TestFixtureSetUp]
        public void Setup()
        {
            _createPostCommand = new CreatePostCommand(_title, _shortUrl);

            _createdPost = Domain.Post.Post.Create(_title, _shortUrl);
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            // Assert
            var changes = _createdPost.GetChanges().ToList();
            changes.Count.Should().Be(1);
            var postCreatedEvent = changes.First() as PostCreatedEvent;
            postCreatedEvent.Should().NotBeNull();
            postCreatedEvent.Title.Should().Be(_title);
            postCreatedEvent.ShortUrl.Should().Be(_shortUrl);
            _createdPost.Version.Should().Be(1);
        }
    }

    public class CreatePostCommand : Command
    {
        private readonly string _title;
        private readonly string _shortUrl;

        public CreatePostCommand(string title, string shortUrl)
            : base(Guid.NewGuid())
        {
            _title = title;
            _shortUrl = shortUrl;
        }
    }

}
