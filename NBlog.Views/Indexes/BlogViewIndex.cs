using System.Linq;
using Raven.Client.Indexes;

namespace NBlog.Views.Indexes
{
    public class BlogViewIndex : AbstractIndexCreationTask<BlogViewItem>
    {
        public BlogViewIndex()
        {
            Map = docs => from doc in docs
                          select new { doc.BlogId};
        }
    }
}