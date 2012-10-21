using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using NBlog.Views;
using NBlog.Web.Models;
using NBlog.Web.Services;

namespace NBlog.Web.Controllers
{
    public partial class PostController : Controller
    {
        private readonly IPostView _postView;
        private readonly IAuthenticationService _authenticationService;

        public PostController(IPostView postView, IAuthenticationService authenticationService)
        {
            _postView = postView;
            _authenticationService = authenticationService;
        }

        public virtual ActionResult Index()
        {
            var items = _postView.GetPublishedPosts();
            return View(items);
        }


        public virtual ActionResult Show(string slug)
        {
            var postItemViewModel = new PostItemViewModel(_postView.GetPostWithSlug(slug));
            postItemViewModel.IsAdminMode = IsAdminMode();
            return View("Show", postItemViewModel);
        }

        private bool IsAdminMode()
        {
            var isAuthenticated = IsUserAuthenticated(User);
            var isRequestingAdminMode = IsRequestingAdminMode();
            return isAuthenticated && isRequestingAdminMode;
        }

        private bool IsRequestingAdminMode()
        {
            return _authenticationService.GetUserMode(Request) == UserMode.Admin;
        }

        private bool IsUserAuthenticated(IPrincipal user)
        {
            return user != null && user.Identity.IsAuthenticated;
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
