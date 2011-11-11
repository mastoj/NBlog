using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Domain.Translators;
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
            var model = _postRepository.All().ToIPosts<PostViewModel>();
            return View("Index", model);
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
            else
            {
                var post = model.ToDTO();
                _postRepository.Insert(post);
                ViewData.AddInfoMessage("Post created!");
                return View("Create");
            }
        }

        public ActionResult Edit(string id)
        {
            var post = _postRepository.Single(y => y.ShortUrl == id);
            if (post.IsNull())
            {
                return new HttpNotFoundResult("Can't find post");
            }
            var model = post.ToIPost<PostViewModel>();
            return View("Edit", model);
        }
    }
}

