using NBlog.Domain.Entities;
using NBlog.Domain.Event;

namespace NBlog.Domain.Builders
{
    public class PostBuilder : IBuild<Post>
    {
        private IDomainEventManager _domainEventHandler;

        public PostBuilder(IDomainEventManager domainEventHandler)
        {
            _domainEventHandler = domainEventHandler;
        }

        public Post Build()
        {
            return new Post(_domainEventHandler);
        }
    }
}