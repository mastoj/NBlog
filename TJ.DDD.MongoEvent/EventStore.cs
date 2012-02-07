using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NBlog.Data.Mongo.Tests.EventRepository;
using TJ.DDD.Infrastructure;
using TJ.Mongo.Util;

namespace TJ.DDD.MongoEvent
{
    public class EventStore : IEventStore
    {
        private MongoServer _server;
        private MongoDatabase _database;

        public EventStore(IMongoConfiguration mongoConfiguration)
        {
            _server = MongoServer.Create(mongoConfiguration.Url);
            var mongoDatabaseSettings = _server.CreateDatabaseSettings(mongoConfiguration.DatabaseName);
            _database = _server.GetDatabase(mongoDatabaseSettings);
        }

        public void Insert(IDomainEvent domainEvent)
        {
            MongoCollection<IDomainEvent> events = _database.GetCollection<IDomainEvent>("Events");
            events.Insert(domainEvent);
        }

        public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            MongoCollection<IDomainEvent> events = _database.GetCollection<IDomainEvent>("Events");
            var query = Query.EQ("AggregateId", aggregateId);
            var cursor = events.FindAs<IDomainEvent>(query);
            foreach (var domainEvent in cursor)
            {
                yield return domainEvent;
            }
        }

        public void DeleteCollection()
        {
            _database.DropCollection("Events");
        }
    }
}