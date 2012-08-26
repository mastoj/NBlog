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
            return View(MVC.Admin.Post.ActionNames.Create, new CreatePostCommand());
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

        public virtual ActionResult Edit(Guid aggregateId)
        {
            var post = _postView.GetPosts().SingleOrDefault(y => y.AggregateId == aggregateId);
            if (post.IsNull())
            {
                return new HttpNotFoundResult("No post with id " + aggregateId);
            }
            var viewModel = new EditPostModel(post);
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Publish(Guid aggregateId)
        {
            var command = new PublishPostCommand(aggregateId);
            return ValidateAndSendEditCommand(command);
        }

        [HttpPost]
        public virtual ActionResult Unpublish(Guid aggregateId)
        {
            var command = new UnpublishPostCommand(aggregateId);
            return ValidateAndSendEditCommand(command);
        }

        [HttpPost]
        public virtual ActionResult Edit(UpdatePostCommand command)
        {
            return ValidateAndSendEditCommand(command);
        }

        private ActionResult ValidateAndSendEditCommand(Command command)
        {
            Func<bool> preCondition = () => _postView.GetPosts().SingleOrDefault(y => y.AggregateId == command.AggregateId).IsNotNull();
            Func<ActionResult> preConditionResult = () => new HttpNotFoundResult("No post with id " + command.AggregateId);
            return ValidateAndSendCommand(command, () => RedirectToAction(MVC.Admin.Post.Index()), () => View(command), preCondition, preConditionResult);
        }
    }
}