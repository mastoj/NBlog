using NBlog.Domain;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NBlog.Views;
using TJ.CQRS.Messaging;
using TJ.CQRS.Respositories;

namespace NBlog.Configuration
{
    public class NBlogDomainConfiguration : INBlogDomainConfiguration
    {
        private IDomainRepository<Post> _postRepository;
        private readonly IDomainRepository<Blog> _blogRepository;
        private readonly IViewRepository<PostItem> _postViewRepostiory;
        private readonly IViewRepository<BlogViewItem> _blogViewRepository;
        private readonly IViewRepository<Author> _authorViewRepository;
        private PostCommandHandlers _postCommandHandlers;
        private PostView _postEventHandlers;
        private BlogCommandHandlers _blogCommandHandlers;
        private BlogView _blogEventHandlers;
        private AuthorView _authorEventHandlers;

        public NBlogDomainConfiguration(IDomainRepository<Post> postRepository, 
            IDomainRepository<Blog> blogRepository, 
            IViewRepository<PostItem> postViewRepostiory, 
            IViewRepository<BlogViewItem> blogViewRepository,
            IViewRepository<Author> authorViewRepository)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _postViewRepostiory = postViewRepostiory;
            _blogViewRepository = blogViewRepository;
            _authorViewRepository = authorViewRepository;
            _postCommandHandlers = new PostCommandHandlers(_postRepository);
            _blogCommandHandlers = new BlogCommandHandlers(_blogRepository);
            _postEventHandlers = new PostView(_postViewRepostiory);
            _blogEventHandlers = new BlogView(_blogViewRepository);
            _authorEventHandlers = new AuthorView(_authorViewRepository);
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
            RegisterAuthorEventHandlers(messageRouter);
        }

        private void RegisterAuthorEventHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<UserAddedEvent>(_authorEventHandlers.Handle);
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
        public AuthorView AuthorView { get { return _authorEventHandlers; } }
    }
}