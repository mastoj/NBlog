using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;

namespace NBlog.Tests
{
    public class InMemoryUserRepository : InMemoryRepository<User>, IUserRepository
    {
    }
}