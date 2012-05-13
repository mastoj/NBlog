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
    public class When_Creating_A_User : BaseCommandTest<CreateUserCommand>
    {
        protected override CreateUserCommand When()
        {
            _createUserCommand = new CreateUserCommand();
            return _createUserCommand;
        }

        private CreateUserCommand _createUserCommand;
    }

    [TestFixture]
    public class When_Creating_The_Blog: BaseCommandTest<CreateBlogCommand>
    {
        protected override CreateBlogCommand When()
        {
            _createBlogCommand = new CreateBlogCommand("Blog title", "Subtitle", "AdminId", "User Name", "author@Name.com");
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
        public void Then_Admin_Author_Should_Be_Added_To_Blog()
        {
            var events = GetPublishedEvents().OfType<UserAddedEvent>();
            events.Count().Should().Be(1);
            AuthorView.GetAuthors().Count().Should().Be(1);
        }

        private CreateBlogCommand _createBlogCommand;
    }
}
