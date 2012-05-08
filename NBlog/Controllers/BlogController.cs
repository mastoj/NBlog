using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NBlog.Models;
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
            var navigationItems = new List<NavigationItem>
                                      {
                                          new NavigationItem() {Slug = "home", Text = "Home"},
                                          new NavigationItem() {Slug = "about", Text = "About"},
                                          new NavigationItem() {Slug = "contact", Text = "Contact"}
                                      };
            return View("_Navigation", navigationItems);
        }
    }
}
