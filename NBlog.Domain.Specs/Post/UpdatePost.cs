using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NUnit.Framework;

namespace NBlog.Domain.Specs.Post.Update
{
    public class When_Updating_A_Post_That_Does_Not_Exist : BaseCommandTest<UpdatePostCommand>
    {
        protected override UpdatePostCommand When()
        {
            Guid aggregateId = Guid.NewGuid();
            var newTitle = "NewTitle";
            var newContent = "NewContent";
            var newSlug = "NewSlug";
            var newTags = new List<string> { "tag4", "tag5", "tag6" };
            var newExcerpt = "NewExcerpt";
            var updatePostCommand = new UpdatePostCommand(newTitle, newContent, newSlug, newTags, newExcerpt,
                                                          aggregateId);
            return updatePostCommand;
        }

        [Test]
        public void An_Post_Does_Not_Exist_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostDoesNotExistException>();
        }
    }

    public class When_Updating_A_Post_That_Exist : BaseCommandTest<UpdatePostCommand>
    {
        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            PreSetCommand(new CreatePostCommand("This exist", "Some serious content", "url", new List<string>() { "tag" }, "excerpt", _aggregateId));
        }

        protected override UpdatePostCommand When()
        {
            _lowestPossibleDate = DateTime.Now;
            _newTitle = "NewTitle";
            _newContent = "NewContent";
            _newSlug = "NewSlug";
            _newTags = new List<string> { "tag4", "tag5", "tag6" };
            _newExcerpt = "NewExcerpt";
            var updatePostCommand = new UpdatePostCommand(_newTitle, _newContent, _newSlug, _newTags, _newExcerpt,
                                                          _aggregateId);
            return updatePostCommand;
        }

        [Test]
        public void The_Post_Should_Be_Updated()
        {
            var updateEvent = GetPublishedEvents().LastOrDefault() as PostUpdatedEvent;
            updateEvent.Should().NotBeNull();
            updateEvent.Content.Should().Be(_newContent);
            updateEvent.Excerpt.Should().Be(_newExcerpt);
            updateEvent.Slug.Should().Be(_newSlug);
            updateEvent.Tags.SequenceEqual(_newTags);
            updateEvent.Title.Should().Be(_newTitle);
            updateEvent.LastSaveTime.Should().BeOnOrAfter(_lowestPossibleDate);
        }

        private string _newTitle;
        private string _newContent;
        private string _newSlug;
        private List<string> _newTags;
        private string _newExcerpt;
        private Guid _aggregateId;
        private DateTime _lowestPossibleDate;
    }
}
