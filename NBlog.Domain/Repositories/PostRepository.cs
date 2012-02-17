using NBlog.Domain.Entities;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Respositories;

namespace NBlog.Domain.Repositories
{
    public class PostRepository : DomainRepository<Post>
    {
        public PostRepository(IEventStore eventStore) : base(eventStore)
        {
        }
    }
}
