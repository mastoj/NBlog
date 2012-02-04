using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Data.Mongo.Event;
using NBlog.Domain.Event;
using NBlog.Domain.Mongo;
using NBlog.Domain.Repositories;
using NUnit.Framework;

namespace NBlog.Data.Mongo.Tests.Event.EventStore
{
    [TestFixture]
    public class When_Saving_An_Event
    {
        private EventStore<MyEvent> _eventStore;
        private MyEvent _testEvent = new MyEvent();
        private IRepository<MyEvent> _repository;


        [TestFixtureSetUp]
        public void Setup()
        {
            IMongoConfiguration configuration = new MongoConfiguration()
                                                    {ConnectionString = "mongodb://localhost/DataTestDB"};
            _repository = new Repository<MyEvent>(configuration);
            _repository.DeleteAll();
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
    { }
}
