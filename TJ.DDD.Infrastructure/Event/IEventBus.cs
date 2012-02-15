using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Event
{
    public interface IEventBus
    {
        void Publish(IEnumerable<IDomainEvent> uncommitedEvents);
    }
}