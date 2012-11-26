using System;
using System.Linq;
using System.Web.Mvc;
using NBlog.Domain.Commands;
using NBlog.Views;
using NBlog.Web.Helpers;
using NBlog.Web.Models;
using NBlog.Web.Services;
using TJ.CQRS.Event;
using TJ.CQRS.Messaging;

namespace NBlog.Web.Controllers
{
    public partial class BlogController : CommandControllerBase
    {
        private readonly ViewManager _viewManager;
        private IAuthenticationService _authenticationService;
        private readonly IEventBus _eventBus;
        private readonly IEventStore _eventStore;
        private IBlogView _blogView;
        private IUserView _userView;

        public BlogController(ViewManager viewManager, IAuthenticationService authenticationService, ICommandBus commandBus, IEventBus eventBus, IEventStore eventStore)
            : base(commandBus)
        {
            _viewManager = viewManager;
            _authenticationService = authenticationService;
            _eventBus = eventBus;
            _eventStore = eventStore;
            _blogView = _viewManager.GetView<IBlogView>();
            _userView = _viewManager.GetView<IUserView>();
        }

        [Authorize]
        public virtual ActionResult Create()
        {
            ActionResult actionResult;
            if (RedirectIfBlogExists(out actionResult)) return actionResult;
            var createBlogCommand = new CreateBlogCommand();
            return View("Create", createBlogCommand);
        }

        [Authorize]
        public ActionResult Edit()
        {
            EditBlogViewModel editBlogViewModel = new EditBlogViewModel();
            var blog = _blogView.GetBlogs().First();
            editBlogViewModel.RedirectUrls = blog.RedirectUrls;
            return View(editBlogViewModel);
        }

        public ActionResult AddRedirectUrls(string oldUrl, string newUrl)
        {
            var blog = _blogView.GetBlogs().First();
            var blogId = blog.BlogId;
            var addRedirectCommand = new AddRedirectUrlCommand(blogId, oldUrl, newUrl);
            return ValidateAndSendCommand(addRedirectCommand, () => RedirectToAction("Edit"), () => RedirectToAction("Edit"));
        }

        private bool RedirectIfBlogExists(out ActionResult actionResult)
        {
            actionResult = null;
            if (_blogView.GetBlogs().Count() > 0)
            {
                {
                    actionResult = RedirectToAction("Index", "Post");
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Create(CreateBlogCommand command)
        {
            ActionResult actionResult;
            if (RedirectIfBlogExists(out actionResult)) return actionResult;
            Func<bool> preCondition = () => _blogView.GetBlogs().Count() == 0;
            var user = _userView.GetUserByAuthenticationId(User.Identity.Name);
            Func<ActionResult> nextResult = () => RedirectToAction("Create", "Post");
            Func<ActionResult> failResult = () => { throw new ApplicationException("Fail to create blog"); };
            Func<ActionResult> validationFailFunc = () => View("Create", command);
            return ValidateAndSendCommand(command, nextResult, failResult, validationFailFunc: validationFailFunc, preCondition: preCondition);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 10)]
        public virtual ActionResult Header()
        {
            var blog = _blogView.GetBlogs().FirstOrDefault() ??
                       new BlogViewItem() {BlogTitle = "Please, ", SubTitle = "Give me a name"};
            return View("_Header", blog);
        }

        [ChildActionOnly]
        public ActionResult AdminMenu()
        {
            var userMode = _authenticationService.GetUserMode(Request);
            return View("_AdminMenu", userMode);
        }

        [Authorize]
        public ActionResult ResetViews()
        {
            foreach (var views in _viewManager.GetAllViews())
            {
                views.ResetView();
            }
            var allEvents = _eventStore.GetAllEvents();
            _eventBus.PublishEvents(allEvents);
            return RedirectToAction("Edit");
        }

        [ChildActionOnly]
        public ActionResult BlogAddonsHeader()
        {
            var blogAddonsViewModel = new BlogAddonsViewModel();
            var blog = _blogView.GetBlogs().FirstOrDefault();
            if(blog != null)
            {
                blogAddonsViewModel.GoogleAnalyticsEnabled = blog.GoogleAnalyticsEnabled;
                blogAddonsViewModel.UAAccount = blog.UAAccount;
                blogAddonsViewModel.DisqusShortName = blog.DisqusShortName;
                blogAddonsViewModel.DisqusEnabled = blog.DisqusEnabled;
            }
            return View("_BlogAddons", blogAddonsViewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EnableGoogleAnalytics(string uaAccount)
        {
            var blog = _blogView.GetBlogs().First();
            var blogId = blog.BlogId;
            var enableGoogleAnalyticsCommand = new EnableGoogleAnalyticsCommand(uaAccount, blogId);
            return ValidateAndSendCommand(enableGoogleAnalyticsCommand, () => RedirectToAction("Edit"),
                                   () => RedirectToAction("Edit"));
        }

        [HttpPost]
        [Authorize]
        public ActionResult EnableDisqusAnalytics(string shortName)
        {
            var blog = _blogView.GetBlogs().First();
            var blogId = blog.BlogId;
            var enableDisqusCommand = new EnableDisqusCommand(blogId, shortName);
            return ValidateAndSendCommand(enableDisqusCommand, () => RedirectToAction("Edit"),
                                   () => RedirectToAction("Edit"));
        }

        [ChildActionOnly]
        public ActionResult AfterContentAddons(Guid postId, string url)
        {
            var blog = _blogView.GetBlogs().First();
            var afterContentViewModel = new AfterContentViewModel()
                                           {
                                               PostId = postId,
                                               DisqusEnabled = blog.DisqusEnabled,
                                               Url = url
                                           };
            return View(afterContentViewModel);
        }
    }
}
