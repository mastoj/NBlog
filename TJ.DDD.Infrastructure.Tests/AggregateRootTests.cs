using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Exceptions;
using TJ.DDD.MongoEvent;

namespace TJ.DDD.Infrastructure.Tests
{
    [TestFixture]
    public class When_I_Do_Something_Illegal
    {
        private StubAggregate _aggregate;

        [TestFixtureSetUp]
        public void Setup()
        {
            _aggregate = new StubAggregate();
        }

        [Test]
        public void Then_I_Get_An_Unregistered_Event_Exception()
        {
            // Assert
            Action act = () => { _aggregate.SomethingIShouldNotDo(); };
            act.ShouldThrow<UnregisteredEventException>();
        }
    }

    public class When_I_Load_Aggregate_From_Events
    {
        private StubAggregate _aggregate;

        [TestFixtureSetUp]
        public void Setup()
        {
            _aggregate = new StubAggregate();
            var aggregateId = Guid.NewGuid();
            var events = new List<IDomainEvent>();
            for (int i = 0; i < 5; i++)
            {
                var validEvent = new ValidEvent();
                validEvent.SetAggregateId(aggregateId);
                validEvent.SetEventNumber(i);
                events.Add(validEvent);
            }
            _aggregate.LoadAggregate(events);
        }

        [Test]
        public void The_Next_Event_Should_Get_Next_Version_Number()
        {
            _aggregate.DoThis();
            _aggregate.GetChanges().First().EventNumber.Should().Be(5);
        }
    }

    public class StubAggregate : AggregateRoot
    {
        public StubAggregate()
        {
            RegisterEventHandler<ValidEvent>(OkAction);
        }

        public void DoThis()
        {
            Apply(new ValidEvent());
        }

        private void OkAction(ValidEvent obj)
        {
        }

        public void SomethingIShouldNotDo()
        {
            Apply(new ShouldNotEvent());
        }
    }

    public class ValidEvent : DomainEventBase
    {
    }

    public class ShouldNotEvent : DomainEventBase
    {
    }
}
