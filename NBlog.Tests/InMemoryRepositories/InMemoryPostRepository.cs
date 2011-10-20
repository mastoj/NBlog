using NBlog.Data.DTO;
using NBlog.Data.Repositories;

namespace NBlog.Tests.Areas.Admin.Controllers
{
    public class InMemoryPostRepository : InMemoryRepository<Post>, IPostRepository
    {
    }
}