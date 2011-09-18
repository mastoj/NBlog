using System.Web.Mvc;
using EasySec.Hashing;
using NBlog.Controllers;
using NBlog.Data;
using NBlog.Models;
using NUnit.Framework;

namespace NBlog.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void CreateAdmin_CreatesAdminForValidInput()
        {
            // arrange
            var createAdminModel = new CreateAdminModel()
            {
                Password = "password",
                PasswordConfirmation = "password",
                UserName = "Admin",
                Name = "Tomas"
            };
            var userRepository = new InMemoryUserRepository();
            var controller = new AccountController(userRepository, new HashGenerator());

            // act
            var result = controller.CreateAdmin(createAdminModel) as RedirectToRouteResult;
            var user = userRepository.Single(y => y.UserName == createAdminModel.UserName);

            // assert
            Assert.IsNotNull(result, "View can't be null");
            Assert.IsNotNull(user, "User was not created");
            Assert.AreNotEqual(createAdminModel.Password, user.PasswordHash);
        }

        [Test]
        public void CreateAdmin_DoesNotCreatesAdminForPasswordMismatch()
        {
            // arrange
            var createAdminModel = new CreateAdminModel()
            {
                Password = "password",
                PasswordConfirmation = "password2",
                UserName = "Admin",
                Name = "Tomas"
            };
            var userRepository = new InMemoryUserRepository();
            userRepository.DeleteAll();
            var controller = new AccountController(userRepository, new HashGenerator());
            controller.ModelState.AddModelError("Password", "Password mismatch");

            // act
            var result = controller.CreateAdmin(createAdminModel) as ViewResult;
            var user = userRepository.Single(y => y.UserName == createAdminModel.UserName);

            // assert
            Assert.IsNotNull(result, "View can't be null");
            Assert.IsNull(user, "User should not be created for password mismatch");
        }

        [Test]
        public void LogIn_ResultsInRedirectToCreateIfNoUserExists()
        {
            // arrange
            var userRepository = new InMemoryUserRepository();
            userRepository.DeleteAll();
            var controller = new AccountController(userRepository, new HashGenerator());

            // act
            var result = controller.LogIn() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result, "Expected redirect to route result when no user exist");
            Assert.AreEqual("CreateAdmin", result.RouteValues["action"].ToString(), "Expected redirect to create admin view");
        }

    }
}
