using System.Linq;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Exceptions;
using NBlog.Domain.Views;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Respositories;
using TJ.Extensions;

namespace NBlog.Domain.CommandHandlers
{
    public class UpdatePostCommandHandler : IHandle<UpdatePostCommand>
    {
        private readonly IDomainRepository<Post> _postRepository;
        private readonly IPostView _postView;

        public UpdatePostCommandHandler(IDomainRepository<Post> postRepository, IPostView postView)
        {
            _postRepository = postRepository;
            _postView = postView;
        }

        public void Handle(UpdatePostCommand updatePostCommand)
        {
            var postWithSameShortUrl =
                _postView.Get().FirstOrDefault(
                    y => y.ShortUrl == updatePostCommand.ShortUrl && y.PostId != updatePostCommand.AggregateId);
            if (postWithSameShortUrl.IsNotNull())
            {
                throw new PostAlreadyExistsForUrlException(updatePostCommand.ShortUrl);
            }
            var post = _postRepository.Get(updatePostCommand.AggregateId);
            if (post.IsNull())
            {
                throw new PostDoesNotExistException();
            }
            post.Update(updatePostCommand.Title, updatePostCommand.Content, updatePostCommand.ShortUrl,
                        updatePostCommand.Tags, updatePostCommand.Excerpt);
        }
    }
}