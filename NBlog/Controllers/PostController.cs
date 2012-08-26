using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Views;

namespace NBlog.Controllers
{
    public partial class PostController : Controller
    {
        private readonly IPostView _postView;

        public PostController(IPostView postView)
        {
            _postView = postView;
        }

        public virtual ActionResult Index()
        {
            var items = _postView.GetPublishedPosts();
            return View(items);
        }


        public virtual ActionResult Show(string slug)
        {
            var postItem = _postView.GetPostWithSlug(slug);
            return View("Show", postItem);
        }

        [ChildActionOnly]
        public virtual ActionResult RecentPosts()
        {
            IEnumerable<PostItem> recentPosts = PostItems().Take(10);
            return PartialView("_RecentPosts", recentPosts);
        }

        private IEnumerable<PostItem> PostItems()
        {
            for (int i = 0; i < 20; i++)
            {
                yield return new PostItem()
                                 {
                                     Excerpt = GenerateListString(20).Aggregate((y, x) => y + x),
                                     Title = GenerateString(),
                                     Slug = "slug" + i,
                                     Content = GenerateListString(60).Aggregate((y, x) => y + x),
                                     PublishedTime = DateTime.UtcNow
                                 };
            }
        }

        private IEnumerable<string> GenerateListString(int numberOfLoremIpsum)
        {
            for (int i = 0; i < numberOfLoremIpsum; i++)
            {
                yield return GenerateString();
            }
        }

        private string GenerateString()
        {
            return "Lorem ipsum ";
        }
    }
}
