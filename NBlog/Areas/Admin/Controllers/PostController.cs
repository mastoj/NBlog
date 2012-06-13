using System;
using System.Linq;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Commands;
using NBlog.Views;
using TJ.CQRS.Messaging;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    [Authorize]
    public partial class PostController : CommandControllerBase
    {
        private readonly IPostView _postView;

        public PostController(IPostView postView, ICommandBus commandBus) : base(commandBus)
        {
            _postView = postView;
        }

        public virtual ActionResult Index(bool? includeDeleted)
        {
            includeDeleted = includeDeleted ?? false;
            var posts = _postView.GetPosts(includeDeleted.Value);
            return View(MVC.Admin.Post.ActionNames.Index, posts);
        }

        public virtual ActionResult Create()
        {
            return View(MVC.Admin.Post.ActionNames.Create);
        }

        [HttpPost]
        public virtual ActionResult Create(CreatePostCommand command)
        {
            return ValidateAndSendCommand(command, () => RedirectToAction(MVC.Admin.Post.Index()), () => View(command));
        }

        [HttpPost]
        public virtual ActionResult CreateAndPublish(CreateAndPublishPostCommand command)
        {
            return ValidateAndSendCommand(command, MVC.Admin.Post.Index, () => View(command));
        }

        public virtual ActionResult Edit(Guid id)
        {
            var post = _postView.GetPosts().SingleOrDefault(y => y.PostId == id);
            if (post.IsNull())
            {
                return new HttpNotFoundResult("No post with id " + id);
            }
            var viewModel = new EditPostModel(post);
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Publish(PublishPostCommand command)
        {
            return ValidateAndSendEditCommand(command);
        }

        [HttpPost]
        public virtual ActionResult Unpublish(UnpublishPostCommand command)
        {
            return ValidateAndSendEditCommand(command);
        }

        [HttpPost]
        public virtual ActionResult Edit(UpdatePostCommand command)
        {
            return ValidateAndSendEditCommand(command);
        }

        private ActionResult ValidateAndSendEditCommand(Command command)
        {
            Func<bool> preCondition = () => _postView.GetPosts().SingleOrDefault(y => y.PostId == command.AggregateId).IsNotNull();
            Func<ActionResult> preConditionResult = () => new HttpNotFoundResult("No post with id " + command.AggregateId);
            return ValidateAndSendCommand(command, MVC.Admin.Post.Index, () => View(command), preCondition, preConditionResult);
        }
    }
}