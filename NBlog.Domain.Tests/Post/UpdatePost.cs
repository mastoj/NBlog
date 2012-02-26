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
            var postView = new StubPostView();
            var updatePostCommandHandler = new UpdatePostCommandHandler(postRepository, postView);
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
            var postView = new StubPostView();
            var updatePostCommandHandler = new UpdatePostCommandHandler(_postRepository, postView);
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

    public class When_Updating_A_Post_That_Exist_To_A_ShorUrl_That_Another_Post_Has : BaseTestSetup
    {
        private Guid _aggregateId;

        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            Entities.Post post = CreatePost();
            var _postRepository = new StubPostRepository();
            _postRepository.Insert(post);
            var _newTitle = "NewTitle";
            var _newContent = "NewContent";
            var _newShortUrl = "NewShortUrl";
            var _newTags = new List<string> { "tag4", "tag5", "tag6" };
            var _newExcerpt = "NewExcerpt";
            var updatePostCommand = new UpdatePostCommand(_newTitle, _newContent, _newShortUrl, _newTags, _newExcerpt,
                                                          _aggregateId);
            var postView = new StubPostView();
            postView.Insert(new PostViewItem() { PostId = _aggregateId, ShortUrl = "shortUrl" });
            postView.Insert(new PostViewItem() { PostId = Guid.Empty, ShortUrl = _newShortUrl });
            var updatePostCommandHandler = new UpdatePostCommandHandler(_postRepository, postView);
            updatePostCommandHandler.Handle(updatePostCommand);
        }

        private Entities.Post CreatePost()
        {
            var post = Entities.Post.Create("Title", "content", "shortUrl", new List<string> { "tag1", "tag2" }, "excerpt", _aggregateId);
            var changes = post.GetChanges();
            post.LoadAggregate(changes);
            post.ClearChanges();
            return post;
        }

        [Test]
        public void An_Post_Already_Exist_For_Url_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<PostAlreadyExistsForUrlException>();
        }
    }
}
