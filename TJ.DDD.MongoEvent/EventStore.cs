using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.Infrastructure.Event;
using TJ.Extensions;
using TJ.Mongo.Util;

namespace TJ.DDD.MongoEvent
{
    public class EventStore : IEventStore, IUnitOfWork
    {
        private MongoServer _server;
        private MongoDatabase _database;
        private string _collectionName = "Events";
        private Dictionary<Guid, AggregateRoot> _aggregateDictionary;

        public EventStore(IMongoConfiguration mongoConfiguration)
        {
            _server = MongoServer.Create(mongoConfiguration.Url);
            var mongoDatabaseSettings = _server.CreateDatabaseSettings(mongoConfiguration.DatabaseName);
            _database = _server.GetDatabase(mongoDatabaseSettings);
            _aggregateDictionary = new Dictionary<Guid, AggregateRoot>();
        }

        public T Get<T>(Guid aggregateId) where T : AggregateRoot, new()
        {
            if (_aggregateDictionary.ContainsKey(aggregateId))
            {
                return _aggregateDictionary[aggregateId] as T;
            }
            var events = GetEvents(aggregateId).ToList();
            T aggregate = new T();
            aggregate.LoadAggregate(events);
            _aggregateDictionary.Add(aggregateId, aggregate);
            return aggregate;
        }

        public void Insert(IDomainEvent domainEvent)
        {
            MongoCollection<IDomainEvent> events = _database.GetCollection<IDomainEvent>(_collectionName);
            events.Insert(domainEvent);
        }

        public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
        {
            MongoCollection<IDomainEvent> events = _database.GetCollection<IDomainEvent>(_collectionName);
            var query = Query.EQ("AggregateId", aggregateId);
            var cursor = events.FindAs<IDomainEvent>(query);
            foreach (var domainEvent in cursor)
            {
                yield return domainEvent;
            }
        }

        public void DeleteCollection()
        {
            _database.DropCollection(_collectionName);
        }

        public void UndoChanges()
        {
            ClearEvents();
        }

        public void Commit()
        {
            MongoCollection<IDomainEvent> events = _database.GetCollection<IDomainEvent>(_collectionName);
            var uncommitedEvents = GetUncommitedEvents();
            events.InsertBatch(uncommitedEvents);
            ClearEvents();
        }

        private void ClearEvents()
        {
            foreach (var aggregateRoot in _aggregateDictionary)
            {
                aggregateRoot.Value.ClearChanges();
            }
        }

        private List<IDomainEvent> GetUncommitedEvents()
        {
            List<IDomainEvent> uncommitedEvents = new List<IDomainEvent>();
            foreach (var aggregateRoot in _aggregateDictionary)
            {
                uncommitedEvents.AddRange(aggregateRoot.Value.GetChanges());
            }
            return uncommitedEvents;
        }
    }
}