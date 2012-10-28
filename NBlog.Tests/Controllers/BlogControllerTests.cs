using System;
using System.Collections.Generic;
using Moq;
using NBlog.Domain.Event;
using NBlog.Views;
using NBlog.Web.Controllers;
using NBlog.Web.Helpers;
using NBlog.Web.Services;
using NUnit.Framework;
using TJ.CQRS.Event;
using TJ.CQRS.Messaging;

namespace NBlog.Tests.Controllers
{
    [TestFixture]
    public class BlogControllerTests
    {
        private BlogController _blogController;
        private Mock<IEventBus> _eventBus;
        private Mock<IEventStore> _eventStore;
        private ICommandBus _commandBus = null;
        private Mock<IUserView> _userView;
        private IAuthenticationService _authenticationService = null;
        private Mock<IBlogView> _blogView;
        private List<IDomainEvent> _events;
        private DateTime _creationTime;
        private Guid _creationGuid;
        private Guid _postId;
        private Mock<IPostView> _postView;

        [TestFixtureSetUp]
        public void When_ResetViews_Views()
        {
            _creationTime = DateTime.Now;
            _creationGuid = Guid.NewGuid();
            _postId = Guid.NewGuid();
            _events = new List<IDomainEvent>()
                {
                    new BlogCreatedEvent("test", "test2", _creationTime, _creationGuid),
                    new PostCreatedEvent("Hej", "hej hej", "slug", new List<string> {"tag"}, "excerpt",
                                         _creationTime.AddDays(1), _postId)
                };

            _blogView = new Mock<IBlogView>();
            _blogView.Setup((y) => y.ResetView());
            _userView = new Mock<IUserView>();
            _userView.Setup((y) => y.ResetView());
            _postView = new Mock<IPostView>();
            _postView.Setup((y) => y.ResetView());
            _eventStore = new Mock<IEventStore>();
            _eventStore.Setup((y) => y.GetAllEvents()).Returns(_events);
            _eventBus = new Mock<IEventBus>();
            _eventBus.Setup((y) => y.PublishEvents(_events));
            var viewManager = new ViewManager(_blogView.Object, _userView.Object, _postView.Object);


            _blogController = new BlogController(viewManager, _authenticationService, _commandBus, _eventBus.Object,
                                                 _eventStore.Object);

            _blogController.ResetViews();
        }

        [Test]
        public void All_Views_Are_Cleared()
        {
            _blogView.VerifyAll();
            _userView.VerifyAll();
        }

        [Test]
        public void And_All_Events_Are_Published_Again()
        {
            _eventBus.VerifyAll();
        }
    }
}
