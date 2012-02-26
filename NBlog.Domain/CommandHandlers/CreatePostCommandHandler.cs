using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Exceptions;
using NBlog.Domain.Repositories;
using NBlog.Domain.Views;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Respositories;
using TJ.Extensions;

namespace NBlog.Domain.CommandHandlers
{
    public class CreatePostCommandHandler : IHandle<CreatePostCommand>
    {
        private readonly IDomainRepository<Post> _postRepository;
        private readonly IPostView _postView;

        public CreatePostCommandHandler(IDomainRepository<Post> postRepository, IPostView postView)
        {
            _postRepository = postRepository;
            _postView = postView;
        }

        public void Handle(CreatePostCommand createPostCommand)
        {
            var postViewItem = _postView.Get().SingleOrDefault(y => y.ShortUrl == createPostCommand.ShortUrl);
            if (postViewItem.IsNotNull())
            {
                throw new PostAlreadyExistsForUrlException(createPostCommand.ShortUrl);
            }
            var post = Post.Create(createPostCommand.Title, createPostCommand.Content, createPostCommand.ShortUrl,
                                        createPostCommand.Tags, createPostCommand.Excerpt, createPostCommand.AggregateId);
            _postRepository.Insert(post);
        }
    }
}
