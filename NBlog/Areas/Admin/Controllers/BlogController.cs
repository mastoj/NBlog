using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Commands;
using NBlog.Views;
using TJ.CQRS.Messaging;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class BlogController : Controller
    {
        private readonly IBlogView _blogView;
        private readonly IAuthorView _authorView;
        private readonly ISendCommand _commandBus;
        //
        // GET: /Admin/Blog/
        public BlogController(IBlogView blogView, IAuthorView authorView, ISendCommand commandBus)
        {
            _blogView = blogView;
            _authorView = authorView;
            _commandBus = commandBus;
        }

        [Authorize]
        public virtual ActionResult Create()
        {
            return View();
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
                var author = _authorView.GetAuthor(User.Identity.Name);
                var createBlogCommand = new CreateBlogCommand(model.BlogTitle, model.SubTitle, author.AuthorId, author.AuthorName, author.AuthorEmail);
                _commandBus.Send(createBlogCommand);
            }
            
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }

    }
}
