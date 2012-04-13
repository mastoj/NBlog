using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Event;
using NUnit.Framework;

namespace NBlog.Views.Tests
{
    [TestFixture]
    public class When_Blog_Is_Created : BlogViewTestBase
    {
        protected override void When()
        {
            _creationTime = DateTime.Now;
            _createdEvent = new BlogCreatedEvent("Title", "SubTitle", _creationTime, Guid.Empty);
            BlogView.Handle(_createdEvent);
        }

        [Test]
        public void Then_It_Is_Added_To_The_View()
        {
            
        }

        private DateTime _creationTime;
        private BlogCreatedEvent _createdEvent;
    }

    [TestFixture]
    public abstract class BlogViewTestBase
    {
        public BlogView BlogView { get; private set; }
        protected virtual void Given()
        {
        }

        protected abstract void When();

        public BlogViewTestBase()
        {
            var blogViewRepository = new InMemoryViewRepository<BlogViewItem>();
            BlogView = new BlogView(blogViewRepository);
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            Given();
            When();
        }
    }
}
