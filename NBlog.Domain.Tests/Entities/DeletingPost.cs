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
    public class When_Deleting_A_Post
    {
        private Post _post;

        [TestFixtureSetUp]
        public void Setup()
        {
            _post = Build.APost();
            _post.ClearChanges();
            _post.Delete();
        }

        [Test]
        public void Then_The_Post_Should_Be_Deleted()
        {
            _post.GetChanges().Count().Should().Be(1);
            _post.GetChanges().First().Should().BeOfType<PostDeletedEvent>();
            (_post.GetChanges().First() as PostDeletedEvent).AggregateId.Should().Be(Guid.Empty);
        }
    }
}
