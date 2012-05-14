using System;
using System.Collections.Generic;
using NBlog.Domain.Specs.Stubs;
using NBlog.Infrastructure;
using NBlog.Views;
using NUnit.Framework;
using TJ.CQRS.Event;
using TJ.CQRS.Messaging;
using TJ.CQRS.Respositories;
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
            var domainRepositoryFactory = new DomainRepositoryFactory(_eventStore);
            var postViewRepository = new InMemoryViewRepository<PostItem>();
            var blogViewRepository = new InMemoryViewRepository<BlogViewItem>();
            var userViewRepository = new InMemoryViewRepository<UserViewItem>();
            var handlerConfiguration = new NBlogDomainConfiguration(domainRepositoryFactory, postViewRepository, blogViewRepository, userViewRepository);
            PostView = handlerConfiguration.PostView;
            BlogView = handlerConfiguration.BlogView;
            UserView = handlerConfiguration.UserView;
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

        protected UserView UserView { get; private set; }


        #endregion
    }
}
