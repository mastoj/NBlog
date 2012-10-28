using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NUnit.Framework;

namespace NBlog.Domain.Specs.Post.Publish
{
    public class When_Setting_The_Publish_Date_On_A_Published_Post : BaseCommandTest<SetPublishDateOnPostCommand>
    {
        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            PreSetCommand(new CreatePostCommand("This exist", "Some serious content", "url", new List<string>() {"tag"}, "excerpt", _aggregateId));
            PreSetCommand(new PublishPostCommand(_aggregateId));
        }

        protected override SetPublishDateOnPostCommand When()
        {
            _publishDate = new DateTime(1982, 3, 8);
            var setPublishDateOnPostCommand = new SetPublishDateOnPostCommand(_aggregateId, _publishDate);
            return setPublishDateOnPostCommand;
        }

        [Test]
        public void A_Publish_Date_Changed_Event_Should_Be_Published()
        {
            var latestEvent = GetPublishedEvents().SingleOrDefault() as PublishDateChangedEvent;
            Assert.IsNotNull(latestEvent);
            Assert.AreEqual(_aggregateId, latestEvent.AggregateId);
            Assert.AreEqual(latestEvent.PublishDate, _publishDate);
        }

        private Guid _aggregateId;
        private DateTime _publishDate;
    }
}
