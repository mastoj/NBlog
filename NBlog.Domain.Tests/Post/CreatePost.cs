using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NUnit.Framework;

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
            _slug = "slug";
            _tags = new List<string>() {"tag1", "tag2"};
            _excerpt = "excerpt";
            var createPostCommand = new CreatePostCommand(_title, _content, _slug, _tags, _excerpt, Guid.NewGuid());
            return createPostCommand;
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            var createPostEvent = GetPublishedEvents().Last() as CreatePostEvent;
            createPostEvent.Content.Should().Be(_content);
            createPostEvent.EventNumber.Should().Be(0);
            createPostEvent.Excerpt.Should().Be(_excerpt);
            createPostEvent.Slug.Should().Be(_slug);
            createPostEvent.Tags.SequenceEqual(_tags);
            createPostEvent.Title.Should().Be(_title);
            createPostEvent.CreationDate.Should().BeOnOrAfter(_lowestPossibleDate);
        }

        private string _title;
        private string _content;
        private string _slug;
        private List<string> _tags;
        private string _excerpt;
        private DateTime _lowestPossibleDate;
    }

    [TestFixture]
    public class When_Creating_A_Post_But_Id_Is_Taken : BaseCommandTest<CreatePostCommand>
    {
        protected override void Given()
        {
            PreSetCommand(new CreatePostCommand("title", "content", "slug", null, "excerpt", Guid.Empty));
        }

        protected override CreatePostCommand When()
        {
            return new CreatePostCommand("title", "content", "slug", null, "excerpt", Guid.Empty);
        }

        [Test]
        public void A_Duplicate_Post_Id_Exception_Is_Thrown()
        {
            CaughtException.Should().BeOfType<DuplicatePostIdException>();
        }
    }

    [TestFixture]
    public class When_Creating_A_Second_Post : BaseCommandTest<CreatePostCommand>
    {
        protected override void Given()
        {
            PreSetCommand(new CreatePostCommand("title", "content", "slug", null, "excerpt", Guid.Empty));
        }

        protected override CreatePostCommand When()
        {
            _lowestPossibleDate = DateTime.Now;
            _title = "Title";
            _content = "content";
            _slug = "slug";
            _tags = new List<string>() { "tag1", "tag2" };
            _excerpt = "excerpt";
            var createPostCommand = new CreatePostCommand(_title, _content, _slug, _tags, _excerpt, Guid.NewGuid());
            return createPostCommand;
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            var createPostEvent = GetPublishedEvents().Last() as CreatePostEvent;
            createPostEvent.Content.Should().Be(_content);
            createPostEvent.EventNumber.Should().Be(0);
            createPostEvent.Excerpt.Should().Be(_excerpt);
            createPostEvent.Slug.Should().Be(_slug);
            createPostEvent.Tags.SequenceEqual(_tags);
            createPostEvent.Title.Should().Be(_title);
            createPostEvent.CreationDate.Should().BeOnOrAfter(_lowestPossibleDate);
        }

        private string _title;
        private string _content;
        private string _slug;
        private List<string> _tags;
        private string _excerpt;
        private DateTime _lowestPossibleDate;
    }
}
