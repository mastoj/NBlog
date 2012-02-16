using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Event;

namespace TJ.DDD.Infrastructure.Tests
{
    [TestFixture]
    public class When_Loading_Aggregate_With_Empty_Event_Store : BaseTestSetup
    {
        protected override void Given()
        {
            IEventBus stubUnitOfWork = new StubEventBus();
            var eventStore = new StubEventStore(stubUnitOfWork);
            eventStore.Get<StubAggregate>(Guid.Empty);
        }

        [Test]
        public void An_Argument_Exception_Should_Be_Thrown()
        {
            CaughtException.Should().BeOfType<ArgumentException>();
        }
    }

    [TestFixture]
    public class When_Commiting_Changes_On_A_Inserted_Aggregate : EventStoreTestBase
    {
        private StubAggregate _aggregate;
        private StubEventBus _stubEventBus;
        private StubEventStore _eventStore;

        protected override void Given()
        {
            _stubEventBus = new StubEventBus();
            _eventStore = new StubEventStore(_stubEventBus);
            _aggregate = new StubAggregate();
            _aggregate.DoThis();
            _aggregate.DoSomethingElse();
            _aggregate.DoThis();
            _aggregate.DoSomethingElse();
            _eventStore.Insert(_aggregate);
            _eventStore.Commit();
        }

        [Test]
        public void The_List_Of_Changes_Should_Be_Empty_On_The_Aggregate()
        {
            _aggregate.GetChanges().Count().Should().Be(0);
        }

        [Test]
        public void The_Uncommited_Events_Should_Be_Published_On_The_Bus_In_The_Right_Order()
        {
            var publishedEvents = _stubEventBus.PublishedEvents.ToList();
            CheckEvents(publishedEvents);
        }

        [Test]
        public void The_Events_Should_Be_Stored()
        {
            var insertedEvents = _eventStore.InsertedEvents.ToList();
            CheckEvents(insertedEvents);
        }
    }

    [TestFixture]
    public class When_Loading_An_Aggregate_With_A_Event_History : EventStoreTestBase
    {
        private StubAggregate _aggregate;

        protected override void Given()
        {
            IEventBus stubUnitOfWork = new StubEventBus();
            var eventStore = new StubEventStore(stubUnitOfWork);
            var events = new List<IDomainEvent>();
            events.Add(new ValidEvent() { AggregateId = Guid.Empty, EventNumber = 0 });
            events.Add(new AnotherValidEvent() { AggregateId = Guid.Empty, EventNumber = 1 });
            events.Add(new ValidEvent() { AggregateId = Guid.Empty, EventNumber = 2 });
            events.Add(new AnotherValidEvent() { AggregateId = Guid.Empty, EventNumber = 3 });
            eventStore.AddEventsForAggregate(Guid.Empty, events);
            _aggregate = eventStore.Get<StubAggregate>(Guid.Empty);
        }

        [Test]
        public void All_The_Events_Should_Be_Applied_To_The_Aggregate()
        {
            var eventsApplied = _aggregate.EventsTriggered;
            CheckEvents(eventsApplied);
        }
    }

    public abstract class EventStoreTestBase : BaseTestSetup
    {
        protected void CheckEvents(List<IDomainEvent> events)
        {
            events.Count.Should().Be(4);
            for (int i = 0; i < events.Count; i++)
            {
                events[i].EventNumber.Should().Be(i);
                if (i % 2 == 0)
                    events[i].GetType().Should().Be(typeof(ValidEvent));
                else
                    events[i].GetType().Should().Be(typeof(AnotherValidEvent));
            }
        }        
    }

    public class StubEventStore : EventStore
    {
        private IEnumerable<IDomainEvent> _insertedEvents;
        private Dictionary<Guid, IEnumerable<IDomainEvent>> _aggregateEventDictionary;

        public StubEventStore(IEventBus eventBus) : base(eventBus)
        {
            _aggregateEventDictionary = new Dictionary<Guid, IEnumerable<IDomainEvent>>();
        }

        public IEnumerable<IDomainEvent> InsertedEvents
        {
            get { return _insertedEvents; }
        }

        protected override void InsertBatch(IEnumerable<IDomainEvent> eventBatch)
        {
            _insertedEvents = eventBatch;
        }

        protected override IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            if (_aggregateEventDictionary.ContainsKey(aggregateId))
            {
                return _aggregateEventDictionary[aggregateId];
            }
            return new List<IDomainEvent>();
        }

        public void AddEventsForAggregate(Guid aggregateId, IEnumerable<IDomainEvent> events)
        {
            _aggregateEventDictionary.Add(aggregateId, events);
        }
    }
}
