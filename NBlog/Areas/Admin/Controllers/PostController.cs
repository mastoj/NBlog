using System;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Helpers;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class PostController : Controller
    {
        public PostController()
        {
        }

        //
        // GET: /Post/

        public virtual ViewResult Index()
        {
//            var model = _postRepository.All().Select(y => new ListPostViewModel() { Post = y});
            return View(Views.Index); //model);
        }

        public virtual ActionResult Create()
        {
            return View(Views.Create, new PostViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid.IsFalse())
            {
                ViewData.AddErrorMessage("Failed to create post");
                return View(Views.Create, model);
            }
            else
            {
                SavePost(model);
                ViewData.AddSuccessMessage("Post created!");
                return View(Views.Create);
            }
        }

        public void SavePost(PostViewModel postViewModel, DateTime? publishDate = null, bool? isPublished = null)
        {
        }

        public virtual ActionResult Edit(string id)
        {
            return View(Views.Edit);
        }
    }
}

