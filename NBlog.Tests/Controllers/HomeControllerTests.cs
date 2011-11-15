using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NBlog.Controllers;
using NUnit.Framework;

namespace NBlog.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void IndexAction_ShouldReturnIndexView()
        {
            // arrange
            var homeController = new HomeController(null);

            // act
            var result = homeController.Index();

            // assert
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
