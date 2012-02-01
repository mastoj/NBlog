using NBlog.Domain.Entities;

namespace NBlog.Domain.Builders
{
    public class PostBuilder : IBuild<Post>
    {
        public Post Build()
        {
            return new Post();
        }
    }
}