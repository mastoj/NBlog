using NBlog.Domain.Entities;
using TJ.CQRS.Event;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.Repositories
{
    public class BlogRepository : DomainRepository<Blog>
    {
        public BlogRepository(IEventStore eventStore)
            : base(eventStore)
        {
        }
    }
}