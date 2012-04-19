using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NUnit.Framework;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.Tests.CommandHandlers
{
    [TestFixture]
    public class When_Create_Blog
    {
        [TestFixtureSetUp]
        public void When()
        {
            _blogRepository = new DomainRepositoryStub<Blog>();
            _blogCommandHandlers = new BlogCommandHandlers(_blogRepository);
            _createBlogCommand = new CreateBlogCommand("Title", "SubTitle", "adminId", "authorName", "author@Name.com");
            _blogCommandHandlers.Handle(_createBlogCommand);
        }

        [Test]
        public void The_Blog_Should_Be_Added_To_The_Repository()
        {
            var blogItem = _blogRepository.Get(_createBlogCommand.AggregateId);
            blogItem.Should().NotBeNull();
        }

        private IDomainRepository<Blog> _blogRepository;
        private BlogCommandHandlers _blogCommandHandlers;
        private CreateBlogCommand _createBlogCommand;
    }
}
