using System.Web.Mvc;
using NBlog.Domain.Commands;
using NBlog.Views;

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
    }
}