using NBlog.Domain.Event;
using NBlog.Views;
using TJ.CQRS.Messaging;

namespace NBlog.Infrastructure.MessageRouting
{
    public class EventRouter : MessageRouter
    {
        private IViewRepository<PostItem> _postViewRepostiory;
        private IViewRepository<BlogViewItem> _blogViewRepository;
        private IViewRepository<UserViewItem> _userViewRepository;
        private PostView _postEventHandlers;
        private BlogView _blogEventHandlers;
        private UserView _userEventHandlers;

        public EventRouter(IViewRepository<PostItem> postViewRepostiory,
                           IViewRepository<BlogViewItem> blogViewRepository,
                           IViewRepository<UserViewItem> userViewRepository)
        {
            _postViewRepostiory = postViewRepostiory;
            _blogViewRepository = blogViewRepository;
            _userViewRepository = userViewRepository;
            _postEventHandlers = new PostView(_postViewRepostiory);
            _blogEventHandlers = new BlogView(_blogViewRepository);
            _userEventHandlers = new UserView(_userViewRepository);
            ConfigureRoutes();
        }

        private void ConfigureRoutes()
        {
            RegisterPostEventHandlers();
            RegisterBlogEventHandlers();
            RegisterUserEventHandlers();
        }

        private void RegisterUserEventHandlers()
        {
            Register<UserCreatedEvent>(_userEventHandlers.Handle);
        }

        private void RegisterBlogEventHandlers()
        {
            Register<BlogCreatedEvent>(_blogEventHandlers.Handle);
        }

        private void RegisterPostEventHandlers()
        {
            Register<PostCreatedEvent>(_postEventHandlers.Handle);
            Register<PostDeletedEvent>(_postEventHandlers.Handle);
            Register<PostPublishedEvent>(_postEventHandlers.Handle);
            Register<PostUpdatedEvent>(_postEventHandlers.Handle);
        }

        // only used for debugging purpose.
        public PostView PostView { get { return _postEventHandlers; } }
        public BlogView BlogView { get { return _blogEventHandlers; } }
        public UserView UserView { get { return _userEventHandlers; } }
    }
}