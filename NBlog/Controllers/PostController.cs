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
    public partial class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public virtual ViewResult Index()
        {
            var posts = _postRepository.All()
                .Where(y => y.Publish && y.PublishDate <= DateTime.Now.Date)
                .OrderByDescending(y => y.PublishDate);
            return View(Views.Index, posts);
        }

        public virtual ActionResult Article(string shorturl)
        {
            var article = _postRepository.All().Where(y => y.ShortUrl == shorturl).FirstOrDefault();
            if (article.IsNull())
            {
                return HttpNotFound("Can't find page");
            }
            return View(Views.Article, article);
        }
    }
}
