using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TJ.DDD.Infrastructure;
using TJ.Extensions;

namespace TJ.DDD.MongoEvent
{
    public abstract class DomainEventBase : IDomainEvent
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public DomainEventBase(Guid aggregateId)
        {
            AggregateId = aggregateId;
            TimeStamp = DateTime.UtcNow;
        }

        public Guid AggregateId { get; set; }

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