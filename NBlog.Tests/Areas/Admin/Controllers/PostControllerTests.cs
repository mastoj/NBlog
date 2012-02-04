using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NBlog.Areas.Admin.Controllers;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Builders;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Helpers;
using NUnit.Framework;

namespace NBlog.Tests.Areas.Admin.Controllers
{
    [TestFixture]
    public class PostControllerTests
    {
        [Test]
        public void Create_ShouldReturnCreateViewWithModel_WhenModelIsInvalid_AndShowErrorFlash()
        {
            // Arrange
            var model = new PostViewModel();
            var target = CreatePostController();
            target.ModelState.AddModelError("Title", "Dummy error to simulate validation error");

            // Act
            var result = target.Create(model) as ViewResult;

            // Assert
            Assert.AreSame(model, result.Model);
            Assert.AreEqual(target.Views.Create, result.ViewName);
            Assert.IsTrue(result.ViewData.GetFlashMessages()["error"].Count > 0);
        }

        [Test]
        public void Create_ShouldReturnCreateViewWithNoModel_WhenModelIsValid_AndShowSuccessFlash()
        {
            // Arrange
            var model = CreateValidPostViewModel();
            var target = CreatePostController();

            // Act
            var result = target.Create(model) as ViewResult;

            // Assert
            Assert.IsNull(result.Model);
            Assert.AreEqual(target.Views.Create, result.ViewName);
            Assert.IsTrue(result.ViewData.GetFlashMessages()["success"].Count > 0);
        }

        [Test]
        public void Create_ShouldCreatePostInRepository_WhenModelIsValid()
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

        [Test]
        public void Index_ModelShouldContainOneElement_IfRepositoryReturnsOneElement()
        {
            // Arrange
            IPostRepository _postRepository = new InMemoryPostRepository();
            var postContent = new PostContent
                                  {
                                      IsPublished = false,
                                      Content = "This is some content"
                                  };
            _postRepository.Insert(new Post()
                                       {
                                           PostMetaData = new PostMetaData
                                                              {
                                                                  ShortUrl = "short url",
                                                                  Title = "This is for real"
                                                              },
                                           Id = Guid.Empty,
                                           PostVersions = new List<PostContent> {postContent}
                                       });
            var target = new PostController(_postRepository);

            // Act
            var result = target.Index() as ViewResult;
            var model = (IEnumerable<ListPostViewModel>)(result.Model);

            // Assert
            Assert.AreEqual(target.Views.Index, result.ViewName);
            Assert.AreEqual(1, model.Count());
            Assert.AreEqual("This is for real", model.First().Post.PostMetaData.Title);
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
            return new PostController(postRepository, null);
        }
    }
}
