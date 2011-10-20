using NBlog.Data.DTO;
using NBlog.Data.Repositories;

namespace NBlog.Data.Mongo.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(IMongoConfiguration mongoConfiguration)
            : base(mongoConfiguration)
        {
            
        }

        public override string CollectionName
        {
            get { return "Posts"; }
        }
    }
}
