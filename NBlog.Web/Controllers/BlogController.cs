using System;
using System.Linq;
using System.Web.Mvc;
using NBlog.Domain.Commands;
using NBlog.Views;
using NBlog.Web.Models;
using NBlog.Web.Services;
using TJ.CQRS.Messaging;

namespace NBlog.Web.Controllers
{
    public partial class BlogController : CommandControllerBase
    {
        private readonly IBlogView _blogView;
        private IAuthenticationService _authenticationService;
        private IUserView _userView;

        public BlogController(IBlogView blogView, IAuthenticationService authenticationService, IUserView userView, ICommandBus commandBus) : base(commandBus)
        {
            _blogView = blogView;
            _authenticationService = authenticationService;
            _userView = userView;
        }

        [Authorize]
        public virtual ActionResult Create()
        {
            ActionResult actionResult;
            if (RedirectIfBlogExists(out actionResult)) return actionResult;
            return View("Create");
        }

        private bool RedirectIfBlogExists(out ActionResult actionResult)
        {
            actionResult = null;
            if (_blogView.GetBlogs().Count() > 0)
            {
                {
                    actionResult = RedirectToAction("Index", "Post");
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Create(CreateBlogModel model)
        {
            ActionResult actionResult;
            if (RedirectIfBlogExists(out actionResult)) return actionResult;
            Func<bool> preCondition = () => _blogView.GetBlogs().Count() == 0;
            var user = _userView.GetUserByAuthenticationId(User.Identity.Name);
            var createBlogCommand = new CreateBlogCommand(model.BlogTitle, model.SubTitle, user.UserId);
            Func<ActionResult> nextResult = () => RedirectToAction("Create", "Post");
            Func<ActionResult> failResult = () => { throw new ApplicationException("Fail to create blog"); };
            Func<ActionResult> validationFailFunc = () => View("Create", model);
            return ValidateAndSendCommand(createBlogCommand, nextResult, failResult, validationFailFunc: validationFailFunc, preCondition: preCondition);
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
