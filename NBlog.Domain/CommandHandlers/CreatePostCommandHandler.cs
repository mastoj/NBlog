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
        private readonly IDomainRepository<Blog> _blogRepository;

        public CreatePostCommandHandler(IDomainRepository<Post> postRepository, IDomainRepository<Blog> blogRepository)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
        }

        public void Execute(CreatePostCommand createPostCommand)
        {
            var post = Post.Create(createPostCommand.Title, createPostCommand.Content, createPostCommand.ShortUrl,
                                        createPostCommand.Tags, createPostCommand.Excerpt, createPostCommand.AggregateId);
            _postRepository.Insert(post);
        }
    }
}
