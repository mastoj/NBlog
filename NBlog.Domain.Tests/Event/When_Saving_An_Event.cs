using System;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.Event;
using NBlog.Domain.Repositories;
using NBlog.Tests;
using NUnit.Framework;

namespace NBlog.Domain.Tests.EventStore
{
    [TestFixture]
    public class When_Saving_An_Event_In_The_Event_Store
    {
        private EventStore<MyEvent> _eventStore;
        private MyEvent _testEvent = new MyEvent(Guid.NewGuid());
        private IRepository<MyEvent> _repository;


        [TestFixtureSetUp]
        public void Setup()
        {
            _repository = new InMemoryRepository<MyEvent>();
            _eventStore = new EventStore<MyEvent>(_repository);
            _eventStore.SaveEvent(_testEvent);
        }

        [Test]
        public void It_Should_Be_Saved_In_The_Repository()
        {
            // Assert
            var savedEvents = _eventStore.GetEvents();
            savedEvents.Count().Should().Be(1);
            savedEvents.First().Should().Be(_testEvent);
        }
    }

    public static class Build
    {
    }

    public class MyEvent : DomainEventBase
    {
        public MyEvent(Guid aggregateId) : base(aggregateId)
        {
            
        }
    }
}
