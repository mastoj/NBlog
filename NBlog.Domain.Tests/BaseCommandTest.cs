﻿using System;
using System.Collections.Generic;
using NBlog.Domain.Repositories;
using NBlog.Domain.Tests.Stubs;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Messaging;
using TJ.DDD.Infrastructure.Tests.Stub;
using TJ.Extensions;

namespace NBlog.Domain.Tests
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
            var handlerConfiguration = new NBlogConfiguration(postRepository);
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
            try
            {
                var commandUnderTest = When();
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
    }
}