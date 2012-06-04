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
    public partial class BlogController : Controller
    {
        private readonly IBlogView _blogView;
        private readonly IUserView _userView;
        private readonly ICommandBus _commandBus;
        //
        // GET: /Admin/Blog/
        public BlogController(IBlogView blogView, IUserView userView, ICommandBus commandBus)
        {
            _blogView = blogView;
            _userView = userView;
            _commandBus = commandBus;
        }

        public virtual ActionResult Index()
        {
            return View(MVC.Admin.Blog.Views.Create);
        }

        [Authorize]
        public virtual ActionResult Create()
        {
            return View(MVC.Admin.Blog.Views.Create);
        }

        [HttpPost]
        public virtual ActionResult Create(CreateBlogModel model)
        {
            var blogs = _blogView.GetBlogs();
            if (blogs.Count() > 0)
            {
                return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
            }
            if (ModelState.IsValid)
            {
                var user = _userView.GetUser(User.Identity.Name);
                var createBlogCommand = new CreateBlogCommand(model.BlogTitle, model.SubTitle, user.UserId);
                _commandBus.Send(createBlogCommand);
            }
            
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name, new { area = MVC.Home.Area });
        }
    }
}
