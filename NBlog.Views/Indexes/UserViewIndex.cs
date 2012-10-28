using System.Linq;
using Raven.Client.Indexes;

namespace NBlog.Views.Indexes
{
    public class UserViewIndex : AbstractIndexCreationTask<UserViewItem>
    {
        public UserViewIndex()
        {
            Map = docs => from doc in docs
                          select new { doc.UserId };            
        }
    }
}