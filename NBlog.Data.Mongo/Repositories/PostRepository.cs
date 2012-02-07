using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;

namespace NBlog.Domain.Mongo.Repositories
{
    public class PostRepository/// : //IPostRepository // Repository<Post>, 
    {
        public PostRepository(IMongoConfiguration mongoConfiguration)
//            : base(mongoConfiguration)
        {
            
        }

        //public override string CollectionName
        //{
        //    get { return "Posts"; }
        //}
    }
}
