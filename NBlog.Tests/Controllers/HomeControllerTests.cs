//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using Moq;
//using NBlog.Controllers;
//using NBlog.Domain.Entities;
//using NBlog.Domain.Repositories;
//using NUnit.Framework;

//namespace NBlog.Tests.Controllers
//{
//    [TestFixture]
//    public class HomeControllerTests
//    {
//        [Test]
//        public void IndexAction_ShouldReturnIndexView()
//        {
//            // arrange
//            var postRepositoryStub = new Mock<IPostRepository>();
//            postRepositoryStub.Setup(y => y.All()).Returns(new List<Post>().AsQueryable());
//            var homeController = new PostController(postRepositoryStub.Object);

//            // act
//            var result = homeController.Index();

//            // assert
//            Assert.AreEqual(homeController.Views.Index, result.ViewName);
//        }
//    }
//}
