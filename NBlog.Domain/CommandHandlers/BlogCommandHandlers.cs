using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using TJ.CQRS;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.CommandHandlers
{
    public class BlogCommandHandlers : IHandle<CreateBlogCommand>
    {
        private readonly IDomainRepository<Blog> _blogRepository;

        public BlogCommandHandlers(IDomainRepositoryFactory repositoryFactory)
        {
            _blogRepository = repositoryFactory.GetDomainRepository<Blog>();
        }

        public void Handle(CreateBlogCommand createBlogCommand)
        {
            var blog = Blog.Create(createBlogCommand.BlogTitle, createBlogCommand.Subtitle, createBlogCommand.UserId, createBlogCommand.AggregateId);
            _blogRepository.Insert(blog);
        }
    }
}