using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Event
{
    public interface IEventHandlerFactory
    {
        IEnumerable<IHandle<IDomainEvent>> GetEventHandlers<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    }
}