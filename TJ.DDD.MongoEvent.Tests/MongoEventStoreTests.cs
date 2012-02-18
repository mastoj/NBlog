using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Tests;
using TJ.Mongo.Util;

namespace NBlog.Data.Mongo.Tests
{
    [TestFixture]
    public class When_Commiting_Changes_For_An_Aggregate
    {
        private Guid _aggregateId;
        private StubAggregate _aggregate;
        private IEventBus _unitOfWork;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Arrange
            _unitOfWork = new StubEventBus();
            var mongoConfig = new MongoConfiguration()
            {
                DatabaseName = "EventTestDB"
            };
            var eventStore = new TJ.DDD.MongoEvent.MongoEventStore(mongoConfig, _unitOfWork);
            eventStore.DeleteCollection();
            _aggregate = new StubAggregate();
            _aggregate.AggregateId = Guid.NewGuid();
            _aggregate.DoThis();
            _aggregate.DoSomethingElse();
            _aggregate.DoThis();
            _aggregate.DoSomethingElse();
            _aggregateId = _aggregate.AggregateId;
            eventStore.Insert(_aggregate);
            eventStore.Commit();
        }

        [Test]
        public void All_Changes_Should_Be_Cleared()
        {
            _aggregate.GetChanges().Count().Should().Be(0);
        }

        [Test]
        public void And_All_Events_Should_Be_Applied_When_Loading_From_EventStore_In_The_Right_Order()
        {
            var mongoConfig = new MongoConfiguration()
            {
                DatabaseName = "EventTestDB"
            };
            var eventStore = new TJ.DDD.MongoEvent.MongoEventStore(mongoConfig, _unitOfWork);
            var loadedAggregate = eventStore.Get<StubAggregate>(_aggregateId);
            var appliedEvents = loadedAggregate.EventsTriggered;
            appliedEvents.Count.Should().Be(4);
            for (int i = 0; i < appliedEvents.Count; i++ )
            {
                appliedEvents[i].EventNumber.Should().Be(i);
                if (i % 2 == 0)
                    appliedEvents[i].GetType().Should().Be(typeof(ValidEvent));
                else
                    appliedEvents[i].GetType().Should().Be(typeof (AnotherValidEvent));
            }
        }
    }
}