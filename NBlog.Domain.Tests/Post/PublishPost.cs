using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NBlog.Domain.Exceptions;
using NBlog.Domain.Tests.Stubs;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Tests;

namespace NBlog.Domain.Tests.Post.Publish
{
    public class When_Publishing_A_Post_That_Does_Not_Exist : BaseTestSetup
    {
        protected override void Given()
        {
            var aggregateId = Guid.NewGuid();
            var postRepository = new StubPostRepository();
            var publishPostCommand = new PublishPostCommand(aggregateId);
            var publishPostCommandHandler = new PublishPostCommandHandler(postRepository);
            publishPostCommandHandler.Execute(publishPostCommand);
        }

        [Test]
        public void A_Post_Does_Not_Exist_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostDoesNotExistException>();
        }
    }

    public class When_Publishing_A_Post_That_Does_Exist : BaseTestSetup
    {
        protected override void Given()
        {
            _lowestPossibleTime = DateTime.Now;
            _aggregateId = Guid.NewGuid();
            Entities.Post post = CreatePost();
            _postRepository = new StubPostRepository();
            _postRepository.Insert(post);
            var publishPostCommand = new PublishPostCommand(_aggregateId);
            var publishPostCommandHandler = new PublishPostCommandHandler(_postRepository);
            publishPostCommandHandler.Execute(publishPostCommand);
        }

        [Test]
        public void The_Post_Should_Be_Published()
        {
            var post = _postRepository.Get(_aggregateId);
            post.Should().NotBeNull();
            var changes = post.GetChanges();
            changes.Count().Should().Be(1);
            var publishEvent = changes.FirstOrDefault() as PublishPostEvent;
            publishEvent.PublishTime.Should().BeAfter(_lowestPossibleTime);
        }

        private Guid _aggregateId;
        private StubPostRepository _postRepository;
        private DateTime _lowestPossibleTime;

        private Entities.Post CreatePost()
        {
            var post = Entities.Post.Create("Title", "content", "shortUrl", new List<string> { "tag1", "tag2" }, "excerpt", _aggregateId);
            var changes = post.GetChanges();
            post.LoadAggregate(changes);
            post.ClearChanges();
            return post;
        }
    }
}
