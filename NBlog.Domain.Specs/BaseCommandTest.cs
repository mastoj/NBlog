using System;
using System.Collections.Generic;
using NBlog.Configuration;
using NBlog.Domain.Repositories;
using NBlog.Domain.Specs.Stubs;
using NBlog.Views;
using NUnit.Framework;
using TJ.CQRS.Event;
using TJ.CQRS.Messaging;
using TJ.Extensions;

namespace NBlog.Domain.Specs
{
    [TestFixture]
    public abstract class BaseCommandTest<TCommand> where TCommand : class, ICommand
    {
        private Exception _caughtException;
        private bool _exceptionIsChecked;
        private bool _exceptionOccured;
        private IEventStore _eventStore;
        private InMemoryBus _bus;

        public BaseCommandTest()
        {
            var messageRouter = new MessageRouter();
            _bus = new InMemoryBus(messageRouter);
            _eventStore = new StubEventStore(_bus);
            var postRepository = new PostRepository(_eventStore);
            var blogRepository = new BlogRepository(_eventStore);
            var postViewRepository = new ViewRepositoryStub<PostItem>();
            var blogViewRepository = new ViewRepositoryStub<BlogViewItem>();
            var authorViewRepository = new ViewRepositoryStub<Author>();
            var handlerConfiguration = new NBlogDomainConfiguration(postRepository, blogRepository, postViewRepository, blogViewRepository, authorViewRepository);
            PostView = handlerConfiguration.PostView;
            BlogView = handlerConfiguration.BlogView;
            AuthorView = handlerConfiguration.AuthorView;
            handlerConfiguration.ConfigureMessageRouter(messageRouter);
        }

        protected Exception CaughtException
        {
            get
            {
                _exceptionIsChecked = true;
                return _caughtException ?? new Exception();
            }
        }

        protected virtual void Given()
        {
        }

        protected abstract TCommand When();

        protected void PreSetCommand(ICommand command)
        {
            _bus.Send(command);
        }

        protected IEnumerable<IDomainEvent> GetPublishedEvents()
        {
            return _bus.PublishedEvents;
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            _exceptionOccured = false;
            _exceptionIsChecked = false;
            Given();
            var commandUnderTest = When();
            try
            {
                _bus.Send(commandUnderTest);
            }
            catch (Exception ex)
            {
                _exceptionOccured = true;
                _caughtException = ex;
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            if (_exceptionOccured && _exceptionIsChecked.IsFalse())
            {
                throw _caughtException;
            }
        }

        #region views


        protected PostView PostView { get; private set; }

        protected BlogView BlogView { get; private set; }

        protected AuthorView AuthorView { get; private set; }


        #endregion
    }
}
