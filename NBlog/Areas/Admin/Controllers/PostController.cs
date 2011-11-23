using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Helpers;
using NBlog.Translators;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        //
        // GET: /Post/

        public virtual ViewResult Index()
        {
            var model = _postRepository.All().ToIPosts<PostViewModel>();
            return View(this.Views.Index, model);
        }

        public virtual ActionResult Create()
        {
            return View(new PostViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(PostViewModel model)
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
                ViewData.AddSuccessMessage("Post created!");
                return View("Create");
            }
        }

        public virtual ActionResult Edit(string id)
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

