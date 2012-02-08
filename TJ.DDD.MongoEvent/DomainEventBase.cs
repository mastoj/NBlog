using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TJ.DDD.Infrastructure;
using TJ.Extensions;

namespace TJ.DDD.MongoEvent
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime TimeStamp { get; set; }

        public DomainEventBase()
        {
            TimeStamp = DateTime.UtcNow;
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public Guid AggregateId { get; private set; }
        public int EventNumber { get; private set; }
        public void SetEventNumber(int eventNumber)
        {
            EventNumber = eventNumber;
        }

        public void SetAggregateId(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if ((obj is DomainEventBase).IsFalse()) return false;
            return Equals((DomainEventBase) obj);
        }

        public bool Equals(DomainEventBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.AggregateId.Equals(AggregateId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return AggregateId.GetHashCode();
            }
        }
    }
}