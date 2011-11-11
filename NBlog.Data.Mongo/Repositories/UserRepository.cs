using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;

namespace NBlog.Domain.Mongo.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoConfiguration mongoConfiguration) : base(mongoConfiguration)
        {
            
        }

        public override string CollectionName
        {
            get { return "Users"; }
        }
    }
}
