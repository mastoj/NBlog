using System.Linq;
using System.Web.Mvc;
using NBlog.Views;
using NBlog.Web.Services;

namespace NBlog.Web.Controllers
{
    public partial class BlogController : Controller
    {
        private readonly IBlogView _blogView;
        private IAuthenticationService _authenticationService;

        public BlogController(IBlogView blogView, IAuthenticationService authenticationService)
        {
            _blogView = blogView;
            _authenticationService = authenticationService;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 10)]
        public virtual ActionResult Header()
        {
            var blog = _blogView.GetBlogs().FirstOrDefault() ??
                       new BlogViewItem() {BlogTitle = "Please, ", SubTitle = "Give me a name"};
            return View("_Header", blog);
        }

        //[ChildActionOnly]
        //[OutputCache(Duration = 10)]
        //public virtual ActionResult Navigation()
        //{
        //    //@Html.ActionLink(item.Text,MVC.Post.ActionNames.Show, MVC.Post.Name, new { slug = item.Url}, null)
        //    var navigationItems = new List<NavigationItem>
        //                              {
        //                                  new NavigationItem() {Url = Url.Action(MVC.Post.ActionNames.Show, MVC.Post.Name,new { slug = "home"}) , Text = "Home"},
        //                                  new NavigationItem() {Url = Url.Action(MVC.Post.ActionNames.Show, MVC.Post.Name,new { slug = "about"}) , Text = "About"},
        //                                  new NavigationItem() {Url = Url.Action(MVC.Post.ActionNames.Show, MVC.Post.Name,new { slug = "contact"}) , Text = "Contact"}
        //                              };
        //    return View("_Navigation", navigationItems);
        //}
        [ChildActionOnly]
        public ActionResult AdminMenu()
        {
            var userMode = _authenticationService.GetUserMode(Request);
            return View("_AdminMenu", userMode);
        }
    }
}
