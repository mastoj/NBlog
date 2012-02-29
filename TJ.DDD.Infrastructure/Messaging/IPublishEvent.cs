using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Messaging
{
    public interface IPublishEvent
    {
        void PublishEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
        void PublishEvents<TEvent>(IEnumerable<TEvent> events) where TEvent : class, IDomainEvent;
    }
}