using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Commands;
using NBlog.Views;
using TJ.CQRS.Messaging;

namespace NBlog.Areas.Admin.Controllers
{
    [Authorize]
    public partial class BlogController : CommandControllerBase
    {
        private readonly IBlogView _blogView;
        private readonly IUserView _userView;
        //
        // GET: /Admin/Blog/
        public BlogController(IBlogView blogView, IUserView userView, ICommandBus commandBus) : base(commandBus)
        {
            _blogView = blogView;
            _userView = userView;
        }

        public virtual ActionResult Index()
        {
            return View(MVC.Admin.Blog.Views.Create);
        }

        public virtual ActionResult Create()
        {
            return View(MVC.Admin.Blog.Views.Create);
        }

        [HttpPost]
        public virtual ActionResult Create(CreateBlogModel model)
        {
            Func<bool> preCondition = () => _blogView.GetBlogs().Count() == 0;
            var user = _userView.GetUserByAuthenticationId(new Guid(User.Identity.Name));
            var createBlogCommand = new CreateBlogCommand(model.BlogTitle, model.SubTitle, user.UserId);
            Func<ActionResult> nextResult = () => RedirectToAction(MVC.Admin.Post.Index());
            Func<ActionResult> failResult = () => RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
            return ValidateAndSendCommand(createBlogCommand, nextResult, failResult, preCondition);
        }
    }
}
