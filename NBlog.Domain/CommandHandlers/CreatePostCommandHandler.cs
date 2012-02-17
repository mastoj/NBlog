using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Respositories;

namespace NBlog.Domain.CommandHandlers
{
    public class CreatePostCommandHandler : IHandle<CreatePostCommand>
    {
        private readonly IDomainRepository<Post> _postRepository;

        public CreatePostCommandHandler(IDomainRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public void Execute(CreatePostCommand createPostCommand)
        {
            var post = Post.Create(createPostCommand.Title, createPostCommand.Content, createPostCommand.ShortUrl,
                                        createPostCommand.Tags, createPostCommand.Excerpt, createPostCommand.AggregateId);
            _postRepository.Insert(post);
        }
    }
}
