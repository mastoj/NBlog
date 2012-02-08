﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure;
using TJ.DDD.MongoEvent;

namespace NBlog.Domain.Tests.Post
{
    [TestFixture]
    public class When_Creating_A_Post
    {
        private string _title;
        private string _shortUrl;
        private Post _createdPost;

        [TestFixtureSetUp]
        public void Setup()
        {
            _createdPost = Post.Create(_title, _shortUrl);
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            // Assert
            var changes = _createdPost.GetChanges().ToList();
            changes.Count.Should().Be(1);
            var postCreatedEvent = changes.First() as PostCreatedEvent;
            postCreatedEvent.Should().NotBeNull();
            postCreatedEvent.Title.Should().Be(_title);
            postCreatedEvent.ShortUrl.Should().Be(_shortUrl);
            _createdPost.Version.Should().Be(1);
        }
    }

    public class Post : AggregateRoot
    {
        private readonly string _title;
        private readonly string _shortUrl;

        public Post()
        {
            
        }

        private Post(string title, string shortUrl) : this()
        {
            _title = title;
            _shortUrl = shortUrl;
            var createEvent = new PostCreatedEvent(title, shortUrl);
            Apply(createEvent);
        }

        public static Post Create(string title, string shortUrl)
        {
            return new Post(title, shortUrl);
        }
    }

    public class PostCreatedEvent : DomainEventBase
    {
        public string Title { get; private set; }
        public string ShortUrl { get; private set; }

        public PostCreatedEvent(string title, string shortUrl)
        {
            Title = title;
            ShortUrl = shortUrl;
        }
    }
}
