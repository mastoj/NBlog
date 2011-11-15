using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Domain.Repositories;
using TJ.Mvc.Filter;

namespace NBlog.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public ViewResult Index()
        {
            return View("Index", _postRepository.All());
        }

        public ActionResult Details(string shorturl)
        {
            throw new NotImplementedException();
        }
    }
}
