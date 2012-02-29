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
    public class When_Creating_A_Post : BaseCommandTest<CreatePostCommand>
    {
        protected override CreatePostCommand When()
        {
            _lowestPossibleDate = DateTime.Now;
            _title = "Title";
            _content = "content";
            _shortUrl = "shortUrl";
            _tags = new List<string>() {"tag1", "tag2"};
            _excerpt = "excerpt";
            var createPostCommand = new CreatePostCommand(_title, _content, _shortUrl, _tags, _excerpt, Guid.NewGuid());
            return createPostCommand;
        }

        [Test]
        public void A_Create_Post_Event_Should_Be_Published()
        {
            var createPostEvent = GetPublishedEvents().First() as CreatePostEvent;
            createPostEvent.Content.Should().Be(_content);
            createPostEvent.EventNumber.Should().Be(0);
            createPostEvent.Excerpt.Should().Be(_excerpt);
            createPostEvent.ShortUrl.Should().Be(_shortUrl);
            createPostEvent.Tags.SequenceEqual(_tags);
            createPostEvent.Title.Should().Be(_title);
            createPostEvent.CreationDate.Should().BeAfter(_lowestPossibleDate);
        }

        private string _title;
        private string _content;
        private string _shortUrl;
        private List<string> _tags;
        private string _excerpt;
        private DateTime _lowestPossibleDate;
    }

    public class When_Creating_A_Post_But_Id_Is_Take : BaseCommandTest<CreatePostCommand>
    {
        protected override void Given()
        {
            PreSetCommand(new CreatePostCommand("title", "content", "shortUrl", null, "excerpt", Guid.Empty));
        }

        protected override CreatePostCommand When()
        {
            return new CreatePostCommand("title", "content", "shortUrl", null, "excerpt", Guid.Empty);
        }

        [Test]
        public void A_Duplicate_Post_Id_Exception_Is_Thrown()
        {
            CaughtException.Should().BeOfType<DuplicatePostIdException>();
        }
    }
}
