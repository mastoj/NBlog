using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Views;

namespace NBlog.Controllers
{
    public partial class HomeController : Controller
    {
        //
        // GET: /Home/

        public virtual ActionResult Index()
        {
            IEnumerable<PostItem> articles = PostItems();
            return View(articles);
        }

        private IEnumerable<PostItem> PostItems()
        {
            for (int i = 0; i < 2   ; i++)
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
