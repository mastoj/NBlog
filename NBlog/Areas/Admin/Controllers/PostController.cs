using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Data.Repositories;
using NBlog.Data;
using NBlog.Helpers;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        //
        // GET: /Post/

        public ViewResult Index()
        {

            return View(new List<PostViewModel>());
        }

        public ActionResult Create()
        {
            return View(new PostViewModel());
        }

        [HttpPost]
        public ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid.IsFalse())
            {
                ViewData.AddErrorMessage("Failed to create post");
                return View("Create", model);
            }
            ViewData.AddInfoMessage("Post created!");
            return View("Create");
        }
    }
}

