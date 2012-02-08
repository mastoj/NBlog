using NBlog.Domain.Entities;
using NBlog.Domain.Event;

namespace NBlog.Domain.Builders
{
    public class PostBuilder : IBuild<Entities.Post>
    {
        private IDomainEventManager _domainEventHandler;

        public PostBuilder(IDomainEventManager domainEventHandler)
        {
            _domainEventHandler = domainEventHandler;
        }

        public Entities.Post Build()
        {
            return new Entities.Post(_domainEventHandler);
        }
    }
}