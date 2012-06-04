using System;
using System.Linq;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Commands;
using NBlog.Views;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class PostController : Controller
    {
        private readonly IPostView _postView;

        public PostController(IPostView postView)
        {
            _postView = postView;
        }

        public virtual ActionResult Index(bool? includeDeleted)
        {
            includeDeleted = includeDeleted ?? false;
            var posts = _postView.GetPosts(includeDeleted.Value);
            return View(MVC.Admin.Post.ActionNames.Index, posts);
        }

        public virtual ActionResult Create()
        {
            return View(MVC.Admin.Post.ActionNames.Create);
        }

        public virtual ActionResult Edit(Guid postId)
        {
            var post = _postView.GetPosts().SingleOrDefault(y => y.PostId == postId);
            if(post.IsNull())
            {
                return new HttpNotFoundResult("No post with id " + postId);
            }
            var viewModel = new EditPostModel(post);
            return View(viewModel);
        }
    }
}