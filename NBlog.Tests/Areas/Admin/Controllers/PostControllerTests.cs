using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NBlog.Areas.Admin.Controllers;
using NBlog.Areas.Admin.Models;
using NBlog.Data.Repositories;
using NBlog.Data;
using NBlog.Helpers;
using NUnit.Framework;

namespace NBlog.Tests.Areas.Admin.Controllers
{
    [TestFixture]
    public class PostControllerTests
    {
        [Test]
        public void Controller_ShouldReturnCreateViewWithModel_WhenModelIsInvalid_AndShowErrorFlash()
        {
            // Arrange
            var model = new PostViewModel();
            var target = CreatePostController();
            target.ModelState.AddModelError("Title", "Dummy error to simulate validation error");

            // Act
            var result = target.Create(model) as ViewResult;

            // Assert
            Assert.AreSame(model, result.Model);
            Assert.AreEqual("Create", result.ViewName);
            Assert.IsTrue(result.ViewData.GetFlashMessages()["error"].Count > 0);
        }

        [Test]
        public void Controller_ShouldReturnCreateViewWithNoModel_WhenModelIsValid_AndShowSuccessFlash()
        {
            // Arrange
            var model = CreateValidPostViewModel();
            var target = CreatePostController();

            // Act
            var result = target.Create(model) as ViewResult;

            // Assert
            Assert.IsNull(result.Model);
            Assert.AreEqual("Create", result.ViewName);
            Assert.IsTrue(result.ViewData.GetFlashMessages()["info"].Count > 0);
        }

        [Test]
        public void Controller_ShouldCreatePostInRepository_WhenModelIsValid()
        {
            // Arrange
            IPostRepository _postRepository = new InMemoryPostRepository(); 
            var model = CreateValidPostViewModel();
            var target = new PostController(_postRepository);

            // Act
            var result = target.Create(model) as ViewResult;

            // Assert
            Assert.AreEqual(1, _postRepository.All().Count());
        }

        private PostViewModel CreateValidPostViewModel()
        {
            return new PostViewModel()
                       {
                           Title = "Title",
                           Content = "Post content",
                           ShortUrl = "ShortUrl"
                       };
        }

        private PostController CreatePostController(IPostRepository postRepository = null)
        {
            postRepository = postRepository ?? new InMemoryPostRepository();
            return new PostController(postRepository);
        }
    }
}
