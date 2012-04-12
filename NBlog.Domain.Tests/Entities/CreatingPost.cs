using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NBlog.Domain.Tests.Builders;
using NUnit.Framework;

namespace NBlog.Domain.Tests.Entities
{
    [TestFixture]
    public class When_Creating_A_Post
    {
        private Post _post;

        [TestFixtureSetUp]
        public void When()
        {
            _post = Build.APost();
        }

        [Test]
        public void Then_An_PostCreatedEvent_Is_Generated()
        {
            var changes = _post.GetChanges();
            changes.Count().Should().Be(1);
            changes.First().Should().BeOfType<PostCreatedEvent>();
        }

        [Test]
        public void Then_All_Properties_Are_Set()
        {
            var postCreatedEvent = _post.GetChanges().First() as PostCreatedEvent;
            postCreatedEvent.Tags.Should().Contain(new List<string> {"tag1", "tag2"});
            postCreatedEvent.Slug.Should().Be("slug");
            postCreatedEvent.Title.Should().Be("title");
            postCreatedEvent.Content.Should().Be("content");
            postCreatedEvent.Excerpt.Should().Be("Excerpt");
            postCreatedEvent.CreationDate.Should().BeAfter(DateTime.Now.AddSeconds(-1));
        }
    }
}
