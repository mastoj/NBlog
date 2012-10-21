using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NUnit.Framework;

namespace NBlog.Domain.Tests.Entities
{
    [TestFixture]
    public class When_Creating_A_Blog
    {
        [TestFixtureSetUp]
        public void When()
        {
            _blog = Blog.Create("Title", "SubTitle", Guid.Empty, Guid.Empty);
        }

        [Test]
        public void Then_A_Blog_Created_Event_Should_Be_Raised()
        {
            var events = _blog.GetChanges().OfType<BlogCreatedEvent>();
            events.Count().Should().Be(1);
            var blogCreatedEvent = events.Single();
            blogCreatedEvent.BlogTitle = "Title";
            blogCreatedEvent.SubTitle = "SubTitle";
        }

        [Test]
        public void Then_A_Admin_Added_Event_Should_Be_Raised()
        {
            var events = _blog.GetChanges().OfType<UserAddedToBlogEvent>();
            events.Count().Should().Be(1);
            var userAddedEvent = events.First();
            userAddedEvent.UserId.Should().Be(Guid.Empty);
        }

        private Blog _blog;
    }
}
