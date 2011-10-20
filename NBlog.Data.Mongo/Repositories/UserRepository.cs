using NBlog.Data.DTO;
using NBlog.Data.Repositories;

namespace NBlog.Data.Mongo.Repositories
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
