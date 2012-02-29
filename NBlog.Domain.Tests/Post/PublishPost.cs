using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NBlog.Domain.Tests.Stubs;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Tests;

namespace NBlog.Domain.Tests.Post.Publish
{
    public class When_Publishing_A_Post_That_Does_Not_Exist : BaseCommandTest<PublishPostCommand>
    {
        protected override PublishPostCommand When()
        {
            var aggregateId = Guid.NewGuid();
            var publishPostCommand = new PublishPostCommand(aggregateId);
            return publishPostCommand;
        }

        [Test]
        public void A_Post_Does_Not_Exist_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostDoesNotExistException>();
        }
    }

    public class When_Publishing_A_Post_That_Does_Exist : BaseCommandTest<PublishPostCommand>
    {
        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            PreSetCommand(new CreatePostCommand("This exist", "Some serious content", "url", new List<string>() {"tag"}, "excerpt", _aggregateId));
        }

        protected override PublishPostCommand When()
        {
            _lowestPossibleTime = DateTime.Now;
            var publishPostCommand = new PublishPostCommand(_aggregateId);
            return publishPostCommand;
        }

        [Test]
        public void The_Post_Should_Be_Published()
        {
            var latestEvent = GetPublishedEvents().LastOrDefault() as PublishPostEvent;
            latestEvent.Should().NotBeNull();
            latestEvent.PublishTime.Should().BeOnOrAfter(_lowestPossibleTime);
            latestEvent.PublishTime.Should().BeOnOrAfter(_lowestPossibleTime);
        }

        private Guid _aggregateId;
        private DateTime _lowestPossibleTime;
    }

    public class When_Publishing_A_Post_That_Is_Already_Published : BaseCommandTest<PublishPostCommand>
    {
        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            PreSetCommand(new CreatePostCommand("This exist", "Some serious content", "url", new List<string>() {"tag"}, "excerpt", _aggregateId));
            PreSetCommand(new PublishPostCommand(_aggregateId));
        }

        protected override PublishPostCommand When()
        {
            var publishPostCommand = new PublishPostCommand(_aggregateId);
            return publishPostCommand;
        }

        [Test]
        public void A_Post_Already_Published_Exception_Should_Be_Raised()
        {
            CaughtException.Should().BeOfType<PostAlreadyPublishedException>();
        }

        private Guid _aggregateId;
    }
}
