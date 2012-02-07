﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Data.Mongo.Tests.EventRepository;
using NUnit.Framework;
using TJ.DDD.MongoEvent;
using TJ.Extensions;

namespace TJ.DDD.Infrastructure.Tests.AggregateRootFactory
{
    [TestFixture]
    public class When_Loading_An_Aggregate_From_History_With_0_Events
    {
        [Test]
        public void An_Argument_Exception_Should_Be_Raised()
        {
            var eventStoreStub = new InMemoryEventStoreStub();
            var aggregateRootFactory = new Infrastructure.AggregateRootFactory(eventStoreStub);
            Action act = () => { aggregateRootFactory.Load<Person>(Guid.Empty); };
            act.ShouldThrow<ArgumentException>();
        }
    }

    [TestFixture]
    public class When_Loading_An_Aggregate_From_History_With_More_Than_0_Events
    {
        private string _expectedName = "Tomas";
        private int _expectedAge = 29;
        private Guid _aggregateId = Guid.NewGuid();
        private Person _person;

        [TestFixtureSetUp]
        public void Setup()
        {
            var eventStoreStub = new InMemoryEventStoreStub();
            eventStoreStub.Insert(
                new SetNameSuccessEvent(_aggregateId) { NewName = _expectedName });
            eventStoreStub.Insert(
                new SetAgeSuccess(_aggregateId) { NewAge = _expectedAge });
            var aggregateRootFactory = new Infrastructure.AggregateRootFactory(eventStoreStub);
            _person = aggregateRootFactory.Load<Person>(_aggregateId);
        }

        [Test]
        public void All_Events_Should_Be_Applied_To_Aggregate()
        {
            _person.Name.Should().Be(_expectedName);
            _person.Age.Should().Be(_expectedAge);
        }
    }

    class InMemoryEventStoreStub : IEventStore
    {
        List<IDomainEvent> _events = new List<IDomainEvent>();

        public void Insert(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            return _events.Where(y => y.AggregateId == aggregateId);
        }
    }

    public class Person : AggregateRoot
    {
        private string _name;
        private int _age;

        public string Name
        {
            get { return _name; }
        }

        public int Age
        {
            get { return _age; }
        }

        public Person()
        {
            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            RegisterEventHandler<SetNameSuccessEvent>(SetNameSuccess);
            RegisterEventHandler<SetAgeSuccess>(SetAgeSuccess);
        }

        void SetNameSuccess(SetNameSuccessEvent setNameSuccessEvent)
        {
            _name = setNameSuccessEvent.NewName;
        }

        void SetAgeSuccess(SetAgeSuccess setAgeSuccess)
        {
            _age = setAgeSuccess.NewAge;
        }
    }

    internal class SetAgeSuccess : DomainEventBase
    {
        public SetAgeSuccess(Guid aggregateId)
            : base(aggregateId)
        {
        }

        public int NewAge { get; set; }
    }

    internal class SetNameSuccessEvent : DomainEventBase
    {
        public SetNameSuccessEvent(Guid aggregateId)
            : base(aggregateId)
        {
        }

        public string NewName { get; set; }
    }
}
