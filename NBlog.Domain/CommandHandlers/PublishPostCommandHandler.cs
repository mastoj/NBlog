using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Exceptions;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Respositories;
using TJ.Extensions;

namespace NBlog.Domain.CommandHandlers
{
    public class PublishPostCommandHandler : IHandle<PublishPostCommand>
    {
        private readonly IDomainRepository<Post> _postRepository;

        public PublishPostCommandHandler(IDomainRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public void Handle(PublishPostCommand publishPostCommand)
        {
            var post = _postRepository.Get(publishPostCommand.AggregateId);
            if (post.IsNull())
            {
                throw new PostDoesNotExistException();
            }
            post.Publish();
        }
    }
}