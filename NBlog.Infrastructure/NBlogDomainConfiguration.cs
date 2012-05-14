using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NBlog.Views;
using TJ.CQRS.Messaging;
using TJ.CQRS.Respositories;

namespace NBlog.Infrastructure
{
    public class NBlogDomainConfiguration : INBlogDomainConfiguration
    {
        private readonly IDomainRepository<Post> _postRepository;
        private readonly IDomainRepository<Blog> _blogRepository;
        private readonly IDomainRepository<User> _userRepository;
        private readonly IViewRepository<PostItem> _postViewRepostiory;
        private readonly IViewRepository<BlogViewItem> _blogViewRepository;
        private readonly IViewRepository<UserViewItem> _userViewRepository;
        private PostCommandHandlers _postCommandHandlers;
        private PostView _postEventHandlers;
        private BlogCommandHandlers _blogCommandHandlers;
        private BlogView _blogEventHandlers;
        private UserView _userEventHandlers;
        private UserCommandHandlers _userCommandHandlers;

        public NBlogDomainConfiguration(IDomainRepository<Post> postRepository,
            IDomainRepository<Blog> blogRepository,
            IDomainRepository<User> userRepository,
            IViewRepository<PostItem> postViewRepostiory, 
            IViewRepository<BlogViewItem> blogViewRepository,
            IViewRepository<UserViewItem> userViewRepository)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userRepository = userRepository;
            _postViewRepostiory = postViewRepostiory;
            _blogViewRepository = blogViewRepository;
            _userViewRepository = userViewRepository;
            _postCommandHandlers = new PostCommandHandlers(_postRepository);
            _blogCommandHandlers = new BlogCommandHandlers(_blogRepository);
            _userCommandHandlers = new UserCommandHandlers(_userRepository);
            _postEventHandlers = new PostView(_postViewRepostiory);
            _blogEventHandlers = new BlogView(_blogViewRepository);
            _userEventHandlers = new UserView(_userViewRepository);
        }

        public void ConfigureMessageRouter(IMessageRouter messageRouter)
        {
            RegisterCommandHandlers(messageRouter);
            RegisterEventHandlers(messageRouter);
        }

        private void RegisterEventHandlers(IMessageRouter messageRouter)
        {
            RegisterPostEventHandlers(messageRouter);
            RegisterBlogEventHandlers(messageRouter);
            RegisterUserEventHandlers(messageRouter);
        }

        private void RegisterUserEventHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<UserCreatedEvent>(_userEventHandlers.Handle);
        }

        private void RegisterBlogEventHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<BlogCreatedEvent>(_blogEventHandlers.Handle);
        }

        private void RegisterPostEventHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<PostCreatedEvent>(_postEventHandlers.Handle);
            messageRouter.Register<PostDeletedEvent>(_postEventHandlers.Handle);
            messageRouter.Register<PostPublishedEvent>(_postEventHandlers.Handle);
            messageRouter.Register<PostUpdatedEvent>(_postEventHandlers.Handle);
        }

        private void RegisterCommandHandlers(IMessageRouter messageRouter)
        {
            RegisterPostCommandHandlers(messageRouter);
            RegisterBlogCommandHandlers(messageRouter);
            RegisterUserCommandHandlers(messageRouter);
        }

        private void RegisterUserCommandHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<CreateUserCommand>(_userCommandHandlers.Handle);
        }

        private void RegisterBlogCommandHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<CreateBlogCommand>(_blogCommandHandlers.Handle);
        }

        private void RegisterPostCommandHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<CreatePostCommand>(_postCommandHandlers.Handle);
            messageRouter.Register<PublishPostCommand>(_postCommandHandlers.Handle);
            messageRouter.Register<UpdatePostCommand>(_postCommandHandlers.Handle);
            messageRouter.Register<DeletePostCommand>(_postCommandHandlers.Handle);
        }

        // only used for debugging purpose.
        public PostView PostView { get { return _postEventHandlers; } }
        public BlogView BlogView { get { return _blogEventHandlers; } }
        public UserView UserView { get { return _userEventHandlers; } }
    }
}
