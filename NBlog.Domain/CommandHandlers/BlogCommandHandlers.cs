using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using TJ.CQRS;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.CommandHandlers
{
    public class BlogCommandHandlers : IHandle<CreateBlogCommand>
    {
        private readonly IDomainRepository<Blog> _blogRepository;

        public BlogCommandHandlers(IDomainRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public void Handle(CreateBlogCommand createBlogCommand)
        {
            var blog = Blog.Create(createBlogCommand.BlogTitle, createBlogCommand.Subtitle, createBlogCommand.AdminId,
                                   createBlogCommand.AuthorName, createBlogCommand.AuthorEmail, createBlogCommand.AggregateId);
            _blogRepository.Insert(blog);
        }
    }
}