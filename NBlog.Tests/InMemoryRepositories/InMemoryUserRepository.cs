using NBlog.Data.DTO;
using NBlog.Data.Repositories;

namespace NBlog.Tests
{
    public class InMemoryUserRepository : InMemoryRepository<User>, IUserRepository
    {
    }
}