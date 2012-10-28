using System.Linq;
using Raven.Client.Indexes;

namespace NBlog.Views.Indexes
{
    public class PostViewIndex : AbstractIndexCreationTask<PostItem>
    {
        public PostViewIndex()
        {
            Map = docs => from doc in docs
                          select new { doc.AggregateId };
        }
    }
}