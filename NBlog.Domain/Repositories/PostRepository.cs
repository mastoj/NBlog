using NBlog.Domain.Entities;
using TJ.CQRS.Event;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.Repositories
{
    public class PostRepository : DomainRepository<Post>
    {
        public PostRepository(IEventStore eventStore) : base(eventStore)
        {
        }
    }
}
