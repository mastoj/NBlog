using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure;
using TJ.DDD.MongoEvent;
using TJ.Mongo.Util;

namespace NBlog.Data.Mongo.Tests.EventRepository
{
    [TestFixture]
    public class When_Adding_A_Event_To_An_Empty_Store
    {
        private List<IDomainEvent> _events;
        private MyEvent _myEvent;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Arrange
            var mongoConfig = new MongoConfiguration()
                                  {
                                      DatabaseName = "EventTestDB"
                                  };
            var eventStore = new EventStore(mongoConfig);
            eventStore.DeleteCollection();
            _myEvent = new MyEvent() { SomeText = "text" };
            _myEvent.SetAggregateId(Guid.NewGuid());
            _myEvent.SetEventNumber(0);

            // Act
            eventStore.Insert(_myEvent);
            _events = eventStore.GetEvents(_myEvent.AggregateId).ToList();
        }

        [Test]
        public void It_Should_Have_A_Count_Of_1()
        {
            // Assert
            _events.Count().Should().Be(1);
        }

        [Test]
        public void It_Should_Contain_The_Event()
        {
            var storedEvent = _events.First();
            storedEvent.Should().Be(_myEvent);
            storedEvent.GetType().Should().Be(typeof(MyEvent));
            (storedEvent as MyEvent).SomeText.Should().Be("text");
        }
    }
}