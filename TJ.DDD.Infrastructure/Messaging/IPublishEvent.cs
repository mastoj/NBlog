namespace TJ.DDD.Infrastructure.Messaging
{
    public interface IPublishEvent
    {
        void Publish<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
    }
}