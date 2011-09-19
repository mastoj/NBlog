using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBlog.Data.Mongo
{
    public class MongoUserRepository : Repository<User>, IUserRepository
    {
        public MongoUserRepository(IMongoConfiguration mongoConfiguration) : base(mongoConfiguration)
        {
            
        }

        public override string CollectionName
        {
            get { return "Users"; }
        }
    }
}
