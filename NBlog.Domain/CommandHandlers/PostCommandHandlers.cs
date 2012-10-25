using System;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Exceptions;
using TJ.CQRS;
using TJ.CQRS.Respositories;
using TJ.Extensions;

namespace NBlog.Domain.CommandHandlers
{
    public class PostCommandHandlers : IHandle<CreatePostCommand>, IHandle<PublishPostCommand>, IHandle<UpdatePostCommand>, IHandle<SetPublishDateOnPostCommand>
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
            var post = TryGetPost(publishPostCommand.AggregateId);
            post.Publish();
        }

        public void Handle(UpdatePostCommand updatePostCommand)
        {
            var post = TryGetPost(updatePostCommand.AggregateId);
            post.Update(updatePostCommand.Title, updatePostCommand.Content, updatePostCommand.Slug,
                        updatePostCommand.Tags, updatePostCommand.Excerpt);
        }

        public void Handle(DeletePostCommand deletePostCommand)
        {
            var post = TryGetPost(deletePostCommand.AggregateId);
            post.Delete();
        }

        public void Handle(SetPublishDateOnPostCommand setPublishDateCommand)
        {
            var post = TryGetPost(setPublishDateCommand.AggregateId);
            post.SetPublishDate(setPublishDateCommand.NewPublishDate);
        }

        private Post TryGetPost(Guid aggregateId)
        {
            var post = _postRepository.Get(aggregateId);
            if (post.IsNull())
            {
                throw new PostDoesNotExistException();
            }
            return post;
        }
    }
}
