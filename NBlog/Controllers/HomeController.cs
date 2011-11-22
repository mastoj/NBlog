using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Domain.Repositories;
using TJ.Extensions;
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
            var posts = _postRepository.All()
                .Where(y => y.Publish && y.PublishDate <= DateTime.Now.Date)
                .OrderByDescending(y => y.PublishDate);
            return View("Index", posts);
        }

        public ActionResult Details(string shorturl)
        {
            throw new NotImplementedException();
        }
    }
}
