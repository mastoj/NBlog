using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using TJ.CQRS;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.CommandHandlers
{
    public class BlogCommandHandlers : IHandle<CreateBlogCommand>, IHandle<EnableGoogleAnalyticsCommand>, IHandle<AddRedirectUrlCommand>, IHandle<EnableDisqusCommand>
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

        public void Handle(EnableGoogleAnalyticsCommand enableGoogleAnalyticsCommand)
        {
            var blog = _blogRepository.Get(enableGoogleAnalyticsCommand.AggregateId);
            blog.EnableGoogleAnalytics(enableGoogleAnalyticsCommand.UAAccount);
        }

        public void Handle(AddRedirectUrlCommand command)
        {
            var blog = _blogRepository.Get(command.AggregateId);
            blog.AddRedirectUrl(command.OldUrl, command.NewUrl);
        }

        public void Handle(EnableDisqusCommand command)
        {
            var blog = _blogRepository.Get(command.AggregateId);
            blog.EnableDisqus(command.ShortName);
        }
    }
}