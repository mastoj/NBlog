using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Event;

namespace TJ.DDD.Infrastructure.Tests
{
    public class EventBusTests : BaseTestSetup
    {
        private EventBus _eventBus;
        private EventHandlerFactory _eventHandlerFactory;

        protected override void Given()
        {
            _eventHandlerFactory = new EventHandlerFactory();
            AddHandlers<ValidEvent>(numberOfHandlers: 2);
            AddHandlers<AnotherValidEvent>(numberOfHandlers: 1);
            _eventBus = new EventBus(_eventHandlerFactory);

            IEnumerable<IDomainEvent> events = new List<IDomainEvent> { new ValidEvent(), new AnotherValidEvent(), new ShouldNotEvent(), new ValidEvent()};
            _eventBus.Publish(events);
        }

        private void AddHandlers<T>(int numberOfHandlers) where T : IDomainEvent
        {
            for (int i = 0; i < numberOfHandlers; i++)
            {
                _eventHandlerFactory.RegisterEventHandler<T>(new EventHandler<IDomainEvent>());
            }
        }

        [Test]
        public void All_Event_Handlers_For_An_Event_Should_Be_Executed()
        {
            var handlers = _eventHandlerFactory.GetEventHandlers(new ValidEvent()).Select(y => y as EventHandler<IDomainEvent>);
            handlers.Count().Should().Be(2);
            foreach (var handler in handlers)
            {
                handler.ExecutionCount.Should().Be(2);
            }
        }

        [Test]
        public void All_Event_Handlers_Should_Be_Executed()
        {
            CheckExecutionFor<ValidEvent>();
            CheckExecutionFor<AnotherValidEvent>();
            CheckExecutionFor<ShouldNotEvent>();
        }

        private void CheckExecutionFor<T>() where T : class, IDomainEvent, new()
        {
            var handlers = _eventHandlerFactory.GetEventHandlers(new T()).Select(y => y as EventHandler<IDomainEvent>);
            foreach (var handler in handlers)
            {
                handler.ExecutionCount.Should().BeGreaterThan(0);
            }
        }
    }

    internal class StubEventHandlerFactory : IEventHandlerFactory
    {
        private Dictionary<Type, object> _handlersDictionary;

        public StubEventHandlerFactory()
        {
            _handlersDictionary = new Dictionary<Type, object>();
        }

        public IEnumerable<IHandle<IDomainEvent>> GetEventHandlers<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            var type = domainEvent.GetType();
            if (_handlersDictionary.ContainsKey(type))
            {
                return _handlersDictionary[type] as List<IHandle<IDomainEvent>>;
            }
            return new List<IHandle<IDomainEvent>>();
        }

        public void AddHandler<T>(int numberOfHandlers) where T : IDomainEvent
        {
            var handlers = new List<IHandle<IDomainEvent>>();
            for (int i = 0; i < numberOfHandlers; i++)
            {
                handlers.Add(new EventHandler<IDomainEvent>());
            }
            _handlersDictionary.Add(typeof (T), handlers);
        }
    }

    internal class EventHandler<T> : IHandle<T> where T : IDomainEvent
    {
        private int _executionCount;

        public int ExecutionCount
        {
            get { return _executionCount; }
        }

        public void Execute(T thingToHandle)
        {
            _executionCount = ExecutionCount + 1;
        }
    }
}
