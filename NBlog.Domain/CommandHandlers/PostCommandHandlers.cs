﻿using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Exceptions;
using TJ.CQRS;
using TJ.CQRS.Respositories;
using TJ.Extensions;

namespace NBlog.Domain.CommandHandlers
{
    public class PostCommandHandlers : IHandle<CreatePostCommand>, IHandle<PublishPostCommand>, IHandle<UpdatePostCommand>
    {
        private readonly IDomainRepository<Post> _postRepository;

        public PostCommandHandlers(IDomainRepositoryFactory repositoryFactory)
        {
            _postRepository = repositoryFactory.GetDomainRepository<Post>();
        }

        public void Handle(CreatePostCommand createPostCommand)
        {

            var post = _postRepository.Get(createPostCommand.AggregateId);
            if (post.IsNotNull())
            {
                throw new DuplicatePostIdException();
            }
            post = Post.Create(createPostCommand.Title, createPostCommand.Content, createPostCommand.Slug,
                               createPostCommand.Tags, createPostCommand.Excerpt, createPostCommand.AggregateId);
            _postRepository.Insert(post);
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

        public void Handle(UpdatePostCommand updatePostCommand)
        {
            var post = _postRepository.Get(updatePostCommand.AggregateId);
            if (post.IsNull())
            {
                throw new PostDoesNotExistException();
            }
            post.Update(updatePostCommand.Title, updatePostCommand.Content, updatePostCommand.Slug,
                        updatePostCommand.Tags, updatePostCommand.Excerpt);
        }

        public void Handle(DeletePostCommand deletePostCommand)
        {
            var post = _postRepository.Get(deletePostCommand.AggregateId);
            if (post.IsNull())
            {
                throw new PostDoesNotExistException();
            }
            post.Delete();
        }
    }
}
