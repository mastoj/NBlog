using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NBlog.Views;

namespace NBlog.Controllers
{
    public partial class BlogController : Controller
    {
        private readonly IBlogView _blogView;

        public BlogController(IBlogView blogView)
        {
            _blogView = blogView;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 10)]
        public virtual ActionResult Header()
        {
            var blog = _blogView.GetBlogs().FirstOrDefault() ??
                       new BlogViewItem() {BlogTitle = "Please, ", SubTitle = "Give me a name"};
            return View("_Header", blog);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 10)]
        public virtual ActionResult Navigation()
        {
            return View("_Navigation");
        }
    }
}
