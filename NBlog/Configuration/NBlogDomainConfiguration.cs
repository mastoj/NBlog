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
        private readonly IPostViewRepostiory _postViewRepostiory;
        private PostCommandHandlers _postCommandHandlers;
        private PostView _postEventHandlers;

        public NBlogDomainConfiguration(IDomainRepository<Post> postRepository, IPostViewRepostiory postViewRepostiory)
        {
            _postRepository = postRepository;
            _postViewRepostiory = postViewRepostiory;
            _postCommandHandlers = new PostCommandHandlers(_postRepository);
            _postEventHandlers = new PostView(_postViewRepostiory);
        }

        public void ConfigureMessageRouter(IMessageRouter messageRouter)
        {
            RegisterCommandHandlers(messageRouter);
            RegisterEventHandlers(messageRouter);
        }

        private void RegisterEventHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<PostCreatedEvent>(_postEventHandlers.Handle);
            messageRouter.Register<PostDeletedEvent>(_postEventHandlers.Handle);
            messageRouter.Register<PostPublishedEvent>(_postEventHandlers.Handle);
        }

        private void RegisterCommandHandlers(IMessageRouter messageRouter)
        {
            messageRouter.Register<CreatePostCommand>(_postCommandHandlers.Handle);
            messageRouter.Register<PublishPostCommand>(_postCommandHandlers.Handle);
            messageRouter.Register<UpdatePostCommand>(_postCommandHandlers.Handle);
            messageRouter.Register<DeletePostCommand>(_postCommandHandlers.Handle);
        }

        // only used for debugging purpose.
        public PostView PostView { get { return _postEventHandlers; } }
    }
}