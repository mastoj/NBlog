using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NUnit.Framework;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Specs.Blog
{
    [TestFixture]
    public class When_Creating_The_Blog: BaseCommandTest<CreateBlogCommand>
    {
        protected override CreateBlogCommand When()
        {
            _createBlogCommand = new CreateBlogCommand("Blog title", "Subtitle", Guid.Empty);
            return _createBlogCommand;
        }

        [Test]
        public void Then_Blog_Should_Be_Created()
        {
            var events = GetPublishedEvents().OfType<BlogCreatedEvent>();
            events.Count().Should().Be(1);
            BlogView.GetBlogs().Count().Should().Be(1);
        }

        [Test]
        public void Then_User_Should_Be_Added_To_Blog()
        {
            var events = GetPublishedEvents().OfType<UserAddedToBlogEvent>();
            events.Count().Should().Be(1);
        }

        private CreateBlogCommand _createBlogCommand;
    }
}
