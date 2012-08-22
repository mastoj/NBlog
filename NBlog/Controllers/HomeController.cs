using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Views;

namespace NBlog.Controllers
{
    public partial class HomeController : Controller
    {
        //
        // GET: /Home/
        private readonly IPostView _postView;

        public HomeController(IPostView postView)
        {
            _postView = postView;
        }

        public virtual ActionResult Index()
        {
            var items = _postView.GetPublishedPosts();
            return View(items);
        }
    }
}
