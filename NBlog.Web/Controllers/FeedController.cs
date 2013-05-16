using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using NBlog.Views;
using NBlog.Web.Helpers;
using NBlog.Web.Models;
using NBlog.Web.Services;

namespace NBlog.Web.Controllers
{
    public class FeedController : Controller
    {
        private IBlogView _blogView;
        private IPostView _postView;

        public FeedController(ViewManager viewManager, IAuthenticationService authenticationService)

        {
            _postView = viewManager.GetView<IPostView>();
            _blogView = viewManager.GetView<IBlogView>();
        }

        public FeedResult Index()
        {
            var blog = _blogView.GetBlogs().First();
            var postItems = _postView.GetPublishedPosts()
                                     .OrderByDescending(p => p.PublishedTime)
                                     .Select(CreateSyndicationItem);

            var title = blog.BlogTitle + (string.IsNullOrEmpty(blog.SubTitle) ? "" : " - " + blog.SubTitle);
            var feed = new SyndicationFeed(title, title, new Uri("http://blog.tomasjansson.com/", UriKind.Absolute), postItems)
            {
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

        private SyndicationItem CreateSyndicationItem(PostItem postItem)
        {
            var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
            var url = new Uri(urlHelper.Action("Show", "Post", new {slug = postItem.Slug}, "http"));
            return new SyndicationItem(postItem.Title, postItem.HtmlExcerpt, url, postItem.Slug, postItem.LastSaveTime);
        }
    }
}
