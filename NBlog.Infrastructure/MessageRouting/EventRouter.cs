using NBlog.Domain.Event;
using NBlog.Views;
using TJ.CQRS;
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

            Register<UserCreatedEvent>(_userEventHandlers.Handle);
            Register<BlogCreatedEvent>(_blogEventHandlers.Handle);
            Register<GoogleAnalyticsEnabledEvent>(_blogEventHandlers.Handle);
            Register<RedirectUrlAddedEvent>(_blogEventHandlers.Handle);
            
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

    public class Logger<T> : IHandle<T> where T : IMessage
    {
        private readonly IHandle<T> _nextStep;

        public Logger(IHandle<T> nextStep)
        {
            _nextStep = nextStep;
        }

        public void Handle(T thingToHandle)
        {
            _nextStep.Handle(thingToHandle);
        }
    }
}