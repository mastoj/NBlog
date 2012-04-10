using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NUnit.Framework;

namespace NBlog.Domain.Tests.Post.Delete
{
    [TestFixture]
    public class When_Deleting_A_Post_That_Does_Not_Exist : BaseCommandTest<DeletePostCommand>
    {
        protected override DeletePostCommand When()
        {
            Guid aggregateId = Guid.NewGuid();
            var deletePostCommand = new DeletePostCommand(aggregateId);
            return deletePostCommand;
        }

        [Test]
        public void An_Post_Does_Not_Exist_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostDoesNotExistException>();
        }
    }

    [TestFixture]
    public class When_Deleting_A_Post_That_Exist : BaseCommandTest<DeletePostCommand>
    {
        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            PreSetCommand(new CreatePostCommand("This exist", "Some serious content", "url", new List<string>() { "tag" }, "excerpt", _aggregateId));
        }

        protected override DeletePostCommand When()
        {
            var deletePostCommand = new DeletePostCommand(_aggregateId);
            return deletePostCommand;
        }

        [Test]
        public void The_Post_Should_Be_Updated()
        {
            var postDeletedEvent = GetPublishedEvents().LastOrDefault() as PostDeletedEvent;
            postDeletedEvent.Should().NotBeNull();
        }

        private Guid _aggregateId;
    }
}
