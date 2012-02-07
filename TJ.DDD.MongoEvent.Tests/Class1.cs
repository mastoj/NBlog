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
            _myEvent = new MyEvent(Guid.NewGuid()) { SomeText = "text" };

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
    [TestFixture]
    public class When_Adding_Two_Different_Events
    {
        private List<IDomainEvent> _events;
        private MyEvent _myEvent;
        private MyEvent2 _myEvent2;

        [TestFixtureSetUp]
        public void Setup()
        {
            // Arrange
            var mongoConfig = new MongoConfiguration()
            {
                DatabaseName = "EventTestDB"
            };
            var eventStore = new EventStore(mongoConfig);
            var aggregateId = Guid.NewGuid();
            _myEvent = new MyEvent(aggregateId) { SomeText = "My EventText" };
            _myEvent2 = new MyEvent2(aggregateId) { SomeText2 = "My Event2Text" };

            // Act
            eventStore.Insert(_myEvent);
            eventStore.Insert(_myEvent2);
            _events = eventStore.GetEvents(aggregateId).ToList();
        }

        [Test]
        public void It_Should_Have_A_Count_Of_2()
        {
            // Assert
            _events.Count().Should().Be(2);
        }

        [Test]
        public void It_Should_Contain_Both_The_Events()
        {
            // Assert
            _events.Contains(_myEvent).Should().BeTrue("Event store should contain event 1");
            _events.Contains(_myEvent2).Should().BeTrue("Event store should contain event 2");
            foreach (var domainEvent in _events)
            {
                if (domainEvent is MyEvent)
                {
                    ((MyEvent)domainEvent).SomeText.Should().Be("My EventText");
                }
                else
                {
                    ((MyEvent2)domainEvent).SomeText2.Should().Be("My Event2Text");
                }
            }
        }
    }

    public class MyEvent : DomainEventBase
    {
        public MyEvent(Guid aggregateId)
            : base(aggregateId)
        {
        }

        public string SomeText { get; set; }
    }

    public class MyEvent2 : DomainEventBase
    {
        public MyEvent2(Guid aggregateId)
            : base(aggregateId)
        {
        }
        public string SomeText2 { get; set; }
    }
}
