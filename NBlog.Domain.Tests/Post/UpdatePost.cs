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
using NBlog.Domain.Views;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Tests;

namespace NBlog.Domain.Tests.Post.Update
{
    public class When_Updating_A_Post_That_Does_Not_Exist : BaseTestSetup
    {
        protected override void Given()
        {
            Guid aggregateId = Guid.NewGuid();
            var postRepository = new StubPostRepository();
            var newTitle = "NewTitle";
            var newContent = "NewContent";
            var newShortUrl = "NewShortUrl";
            var newTags = new List<string> { "tag4", "tag5", "tag6" };
            var newExcerpt = "NewExcerpt";
            var updatePostCommand = new UpdatePostCommand(newTitle, newContent, newShortUrl, newTags, newExcerpt,
                                                          aggregateId);
            var updatePostCommandHandler = new PostCommandHandlers(postRepository);
            updatePostCommandHandler.Handle(updatePostCommand);
        }

        [Test]
        public void An_Post_Does_Not_Exist_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostDoesNotExistException>();
        }
    }

    public class When_Updating_A_Post_That_Exist : BaseTestSetup
    {
        protected override void Given()
        {
            _lowestPossibleDate = DateTime.Now;
            _aggregateId = Guid.NewGuid();
            Entities.Post post = CreatePost();
            _postRepository = new StubPostRepository();
            _postRepository.Insert(post);
            _newTitle = "NewTitle";
            _newContent = "NewContent";
            _newShortUrl = "NewShortUrl";
            _newTags = new List<string> { "tag4", "tag5", "tag6" };
            _newExcerpt = "NewExcerpt";
            var updatePostCommand = new UpdatePostCommand(_newTitle, _newContent, _newShortUrl, _newTags, _newExcerpt,
                                                          _aggregateId);
            var updatePostCommandHandler = new PostCommandHandlers(_postRepository);
            updatePostCommandHandler.Handle(updatePostCommand);
        }

        [Test]
        public void The_Post_Should_Be_Updated()
        {
            var post = _postRepository.Get(_aggregateId);
            post.GetChanges().Count().Should().Be(1);
            var updateEvent = post.GetChanges().FirstOrDefault() as UpdatePostEvent;
            updateEvent.Should().NotBeNull();
            updateEvent.Content.Should().Be(_newContent);
            updateEvent.Excerpt.Should().Be(_newExcerpt);
            updateEvent.ShortUrl.Should().Be(_newShortUrl);
            updateEvent.Tags.SequenceEqual(_newTags);
            updateEvent.Title.Should().Be(_newTitle);
            updateEvent.LastSaveTime.Should().BeAfter(_lowestPossibleDate);
        }

        private Entities.Post CreatePost()
        {
            var post = Entities.Post.Create("Title", "content", "shortUrl", new List<string> { "tag1", "tag2" }, "excerpt", _aggregateId);
            var changes = post.GetChanges();
            post.LoadAggregate(changes);
            post.ClearChanges();
            return post;
        }

        private string _newTitle;
        private string _newContent;
        private string _newShortUrl;
        private List<string> _newTags;
        private string _newExcerpt;
        private StubPostRepository _postRepository;
        private Guid _aggregateId;
        private DateTime _lowestPossibleDate;
    }
}
