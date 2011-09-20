using System.Web.Mvc;
using EasySec.Hashing;
using Moq;
using NBlog.Controllers;
using NBlog.Data;
using NBlog.Infrastructure;
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
            var controller = CreateAccountController(userRepository: userRepository);

            // act
            var result = controller.CreateAdmin(createAdminModel) as RedirectToRouteResult;
            var user = userRepository.Single(y => y.UserName == createAdminModel.UserName);

            // assert
            Assert.IsNotNull(result, "View can't be null");
            Assert.IsNotNull(user, "User was not created");
            Assert.AreNotEqual(createAdminModel.Password, user.PasswordHash);
        }

        private AccountController CreateAccountController(InMemoryUserRepository userRepository = null, IHashGenerator hashGenerator = null, IAuthenticationManager authenticationManager = null)
        {
            userRepository = userRepository ?? new InMemoryUserRepository();
            hashGenerator = hashGenerator ?? new HashGenerator();
            return new AccountController(userRepository, hashGenerator, authenticationManager);
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
            var controller = CreateAccountController();
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
            var controller = CreateAccountController();

            // act
            var result = controller.LogIn() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result, "Expected redirect to route result when no user exist");
            Assert.AreEqual("CreateAdmin", result.RouteValues["action"].ToString(), "Expected redirect to create admin view");
        }

        [Test]
        public void LogIn_WithValidCredentialsResultsInLoggedInUserAndRedirect()
        {
            // arrange
            var hashGenerator = new HashGenerator();
            var user = new User()
            {
                UserName = "admin",
                PasswordHash = hashGenerator.GenerateHash("Password!"),
                Name = "Tomas"
            };
            var userRepository = new InMemoryUserRepository();
            userRepository.Insert(user);
            var userViewModel = new LogInViewModel { UserName = "admin", Password = "Password!" };
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(userRepository: userRepository, authenticationManager: authenticationManager);

            // act 
            var result = controller.LogIn(userViewModel) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", result.RouteValues["area"]);
            mock.Verify(y => y.SignInUser(userViewModel.UserName), Times.AtMostOnce());
        }

        [Test]
        public void LogIn_WithInvalidUserNameResultsInNotLoggedInUserAndSameView()
        {
            // arrange
            var userRepository = new InMemoryUserRepository();
            var userViewModel = new LogInViewModel { UserName = "admin", Password = "Password!" };
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(userRepository: userRepository, authenticationManager: authenticationManager);

            // act 
            var result = controller.LogIn(userViewModel) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            mock.Verify(y => y.SignInUser(userViewModel.UserName), Times.Never());
        }

        [Test]
        public void LogIn_WithInvalidPasswordResultsInNotLoggedInUserAndSameView()
        {
            // arrange
            var hashGenerator = new HashGenerator();
            var user = new User()
            {
                UserName = "admin",
                PasswordHash = hashGenerator.GenerateHash("Password!"),
                Name = "Tomas"
            };
            var userRepository = new InMemoryUserRepository();
            userRepository.Insert(user);
            var userViewModel = new LogInViewModel { UserName = "admin", Password = "WrongPassword!" };
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(userRepository: userRepository, authenticationManager: authenticationManager);

            // act 
            var result = controller.LogIn(userViewModel) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            mock.Verify(y => y.SignInUser(userViewModel.UserName), Times.Never());
        }
    }
}
