using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using TJ.CQRS.Messaging;
using TJ.CQRS.Respositories;

namespace NBlog.Infrastructure.MessageRouting
{
    public class CommandRouter : MessageRouter
    {
        private readonly IDomainRepositoryFactory _domainRepositoryFactory;
        private PostCommandHandlers _postCommandHandlers;
        private BlogCommandHandlers _blogCommandHandlers;
        private UserCommandHandlers _userCommandHandlers;

        public CommandRouter(IDomainRepositoryFactory domainRepositoryFactory)
        {
            _domainRepositoryFactory = domainRepositoryFactory;
            _postCommandHandlers = new PostCommandHandlers(_domainRepositoryFactory);
            _blogCommandHandlers = new BlogCommandHandlers(_domainRepositoryFactory);
            _userCommandHandlers = new UserCommandHandlers(_domainRepositoryFactory);
            ConfigureMessageRouter();
        }

        private void ConfigureMessageRouter()
        {
            RegisterCommandHandlers();
        }

        private void RegisterCommandHandlers()
        {
            RegisterPostCommandHandlers();
            RegisterBlogCommandHandlers();
            RegisterUserCommandHandlers();
        }

        private void RegisterUserCommandHandlers()
        {
            Register<CreateUserCommand>(_userCommandHandlers.Handle);
        }

        private void RegisterBlogCommandHandlers()
        {
            Register<CreateBlogCommand>(_blogCommandHandlers.Handle);
        }

        private void RegisterPostCommandHandlers()
        {
            Register<CreatePostCommand>(_postCommandHandlers.Handle);
            Register<PublishPostCommand>(_postCommandHandlers.Handle);
            Register<UpdatePostCommand>(_postCommandHandlers.Handle);
            Register<DeletePostCommand>(_postCommandHandlers.Handle);
        }
    }
}