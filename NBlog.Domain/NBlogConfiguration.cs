using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using TJ.CQRS.Messaging;
using TJ.CQRS.Respositories;

namespace NBlog.Domain
{
    public class NBlogConfiguration
    {
        private IDomainRepository<Post> _postRepository;

        public NBlogConfiguration(IDomainRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public void ConfigureMessageRouter(IMessageRouter messageRouter)
        {
            var postCommandHandlers = new PostCommandHandlers(_postRepository);
            messageRouter.Register<CreatePostCommand>(postCommandHandlers.Handle);
            messageRouter.Register<PublishPostCommand>(postCommandHandlers.Handle);
            messageRouter.Register<UpdatePostCommand>(postCommandHandlers.Handle);
            messageRouter.Register<DeletePostCommand>(postCommandHandlers.Handle);
        }
    }
}
