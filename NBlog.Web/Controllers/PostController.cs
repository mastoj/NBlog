﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using NBlog.Domain.Commands;
using NBlog.Views;
using NBlog.Web.Models;
using NBlog.Web.Services;
using TJ.CQRS.Messaging;
using TJ.Extensions;
using NBlog.Web.Helpers;

namespace NBlog.Web.Controllers
{
    public class PostController : CommandControllerBase
    {
        private readonly IPostView _postView;
        private readonly IBlogView _blogView;
        private readonly IAuthenticationService _authenticationService;

        public PostController(ViewManager viewManager, IAuthenticationService authenticationService, ICommandBus commandBus)
            : base(commandBus)
        {
            _postView = viewManager.GetView<IPostView>();
            _blogView = viewManager.GetView<IBlogView>();
            _authenticationService = authenticationService;
        }

        public virtual ActionResult Index()
        {
            var blog = _blogView.GetBlogs().FirstOrDefault();
            var items = (_authenticationService.IsUserAuthenticated(User)
                                   ?  _postView.GetPosts()
                                   : _postView.GetPublishedPosts()).OrderByDescending(y => y.PublishedTime);
            var title = blog.BlogTitle;
            if (!string.IsNullOrEmpty(blog.SubTitle))
            {
                title += " - " + blog.SubTitle;
            }

            var model = new PostIndexViewModel()
                            {
                                BlogTitle = title,
                                Posts = items
                            };
            return View(model);
        }

        public ActionResult RedirectUrl(string oldUrl)
        {
            var redirectUrls = _blogView.GetBlogs().First().RedirectUrls;
            if (redirectUrls != null && redirectUrls.Count > 0 && redirectUrls.ContainsKey(oldUrl))
            {
                return RedirectToActionPermanent("Show", new { slug = redirectUrls[oldUrl] });
            }
            return new HttpNotFoundResult();
        }

        public virtual ActionResult Show(string slug)
        {
            var redirectSlug = GetRedirectSlug(slug);
            if (!string.IsNullOrEmpty(redirectSlug))
            {
                return RedirectToAction("Show", "Post", new { slug = redirectSlug });
            }
            PostItem postItem = null;
            int retries = 0;
            do
            {
                postItem = _postView.GetPostWithSlug(slug);
                if (postItem != null)
                    break;
                Thread.Sleep(500);
                retries = retries + 1;
            } while (retries < 3);
            var postItemViewModel = new PostItemViewModel(postItem);
            postItemViewModel.IsAdminMode = IsAdminMode();
            return View("Show", postItemViewModel);
        }

        private string GetRedirectSlug(string slug)
        {
            var blog = _blogView.GetBlogs().First();
            if (blog.RedirectUrls != null && blog.RedirectUrls.ContainsKey(slug))
            {
                return blog.RedirectUrls[slug];
            }
            return null;
        }

        [Authorize]
        public ActionResult Create()
        {
            var command = new CreatePostCommand() {AggregateId = Guid.NewGuid()};
            return View("Create", command);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreatePostCommand command)
        {
            Func<bool> preCondition = () => ValidateSlug(command.AggregateId, command.Slug);

            Func<ActionResult> preConditionResult = () =>
                {
                    ModelState.AddModelError("Slug", "Slug already taken");
                    return View(command);
                };
            return ValidateAndSendCommand(command, () => RedirectToAction("Show", "Post", new { slug = command.Slug}), () => View(command), preCondition: preCondition, preConditionResult: preConditionResult);
        }

        private bool ValidateSlug(Guid aggregateId, string slug)
        {
            var existingPost = _postView.GetPosts().Where(y => y.Slug == slug).FirstOrDefault();
            if(existingPost.IsNotNull() && existingPost.AggregateId != aggregateId)
            {
                return false;
            }
            return true;
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

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(UpdatePostCommand command)
        {
            return ValidateAndSendCommand(command, () => RedirectToAction("Show", "Post", new { slug = command.Slug }), () =>
                { throw new ApplicationException("Failed to update post"); });
        }

        [Authorize]
        public ActionResult Publish(Guid aggregateid)
        {
            var command = new PublishPostCommand(aggregateid);
            var postItem = _postView.GetPosts().Where(y => y.AggregateId == aggregateid).First();
            return ValidateAndSendCommand(command, () => RedirectToAction("Show", "Post", new { slug = postItem.Slug }), () =>
            { throw new ApplicationException("Failed to publish post"); });
        }
    }
}
