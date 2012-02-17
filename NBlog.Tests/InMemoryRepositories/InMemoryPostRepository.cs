using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;

namespace NBlog.Tests.Areas.Admin.Controllers
{
    public class InMemoryPostRepository : InMemoryRepository<Post>
    {
    }
}