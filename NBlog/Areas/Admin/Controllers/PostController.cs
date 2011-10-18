using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;

namespace NBlog.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
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
    }
}

