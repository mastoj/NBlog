using System;
using System.Collections.Generic;
using NBlog.Domain.Specs.Stubs;
using NBlog.Infrastructure;
using NBlog.Infrastructure.MessageRouting;
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
        private StubEventStore _eventStore;
        private InMemoryCommandBus _commandBus;
        private InMemoryEventBus _eventBus;

        public BaseCommandTest()
        {
            var postViewRepository = new InMemoryViewRepository<PostItem>();
            var blogViewRepository = new InMemoryViewRepository<BlogViewItem>();
            var userViewRepository = new InMemoryViewRepository<UserViewItem>();
            var eventRouter = new EventRouter(postViewRepository, blogViewRepository, userViewRepository);
            _eventBus = new InMemoryEventBus(eventRouter);
            _eventStore = new StubEventStore(_eventBus);
            var domainRepositoryFactory = new DomainRepositoryFactory(_eventStore);
            var commandRouter = new CommandRouter(domainRepositoryFactory);
            _commandBus = new InMemoryCommandBus(commandRouter, new UnitOfWork(_eventStore));
            PostView = eventRouter.PostView;
            BlogView = eventRouter.BlogView;
            UserView = eventRouter.UserView;
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
            _commandBus.Send(command);
        }

        protected IEnumerable<IDomainEvent> GetPublishedEvents()
        {
            return _eventBus.PublishedEvents;
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
                _commandBus.Send(commandUnderTest);
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
