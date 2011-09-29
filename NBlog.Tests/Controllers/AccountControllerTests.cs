﻿using System.Linq;
using System.Web.Mvc;
using EasySec.Hashing;
using Moq;
using NBlog.Controllers;
using NBlog.Data;
using NBlog.Data.Extensions;
using NBlog.Infrastructure;
using NBlog.Models;
using NUnit.Framework;

namespace NBlog.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void CreateAdmin_CreatesSignInAndRedirectAdminForValidInput()
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
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(userRepository: userRepository, authenticationManager: authenticationManager);

            // act
            var result = controller.CreateAdmin(createAdminModel) as RedirectToRouteResult;
            var user = userRepository.Single(y => y.UserName == createAdminModel.UserName);

            // assert
            Assert.IsNotNull(result, "View can't be null");
            Assert.IsNotNull(user, "User was not created");
            Assert.AreNotEqual(createAdminModel.Password, user.PasswordHash);
            mock.Verify(y => y.LoginUser(createAdminModel.UserName));
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
        public void Login_ResultsInRedirectToCreateIfNoUserExists()
        {
            // arrange
            var userRepository = new InMemoryUserRepository();
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(userRepository: userRepository, authenticationManager: authenticationManager);

            // act
            var result = controller.Login() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result, "Expected redirect to route result when no user exist");
            Assert.AreEqual("CreateAdmin", result.RouteValues["action"].ToString(), "Expected redirect to create admin view");
            mock.Verify(y => y.LogoutUser(), Times.AtLeastOnce());
        }

        [Test]
        public void Create_ResultsInRedirectToLoginIfUserExists()
        {
            // arrange
            var userRepository = new InMemoryUserRepository();
            var user = new User()
            {
                UserName = "admin",
                PasswordHash = "pasasasasdas",
                Name = "Tomas"
            };
            userRepository.Insert(user);
            var controller = CreateAccountController(userRepository: userRepository);

            // act
            var result = controller.CreateAdmin() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result, "Expected redirect to route result when no user exist");
            Assert.AreEqual("Login", result.RouteValues["action"].ToString(), "Expected redirect to login view");
        }

        [Test]
        public void Create_FailsIfUserExists()
        {
            // arrange
            var userRepository = new InMemoryUserRepository();
            var user = new User()
            {
                UserName = "admin",
                PasswordHash = "pasasasasdas",
                Name = "Tomas"
            };
            var createAdminModel = new CreateAdminModel()
            {
                Password = "password",
                PasswordConfirmation = "password2",
                UserName = "Admin",
                Name = "Tomas"
            };
            userRepository.Insert(user);
            var controller = CreateAccountController(userRepository: userRepository);

            // act
            var result = controller.CreateAdmin(createAdminModel) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result, "Expected redirect to route result when no user exist");
            Assert.AreEqual("Login", result.RouteValues["action"].ToString(), "Expected redirect to login view");
            Assert.AreEqual(1, userRepository.All().Count());
            var userInDb = userRepository.Single();
            Assert.AreEqual(user.UserName, userInDb.UserName);
            Assert.AreEqual(user.PasswordHash, userInDb.PasswordHash);
            Assert.AreEqual(user.Name, userInDb.Name);
        }

        [Test]
        public void Login_WithValidCredentialsResultsInLoggedInUserAndRedirect()
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
            var result = controller.Login(userViewModel) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", result.RouteValues["area"]);
            mock.Verify(y => y.LoginUser(userViewModel.UserName), Times.AtMostOnce());
        }

        [Test]
        public void Login_WithInvalidUserNameResultsInNotLoggedInUserAndSameView()
        {
            // arrange
            var userRepository = new InMemoryUserRepository();
            var userViewModel = new LogInViewModel { UserName = "admin", Password = "Password!" };
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(userRepository: userRepository, authenticationManager: authenticationManager);

            // act 
            var result = controller.Login(userViewModel) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            mock.Verify(y => y.LoginUser(userViewModel.UserName), Times.Never());
        }

        [Test]
        public void Login_WithInvalidPasswordResultsInNotLoggedInUserAndSameView()
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
            var result = controller.Login(userViewModel) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            mock.Verify(y => y.LoginUser(userViewModel.UserName), Times.Never());
        }

        [Test]
        public void Logout_ShouldLogOutUser()
        {
            // arrange
            var mock = new Mock<IAuthenticationManager>();
            var authenticationManager = mock.Object;
            var controller = CreateAccountController(authenticationManager: authenticationManager);

            // act 
            var result = controller.Logout() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            mock.Verify(y => y.LogoutUser(), Times.AtLeastOnce());
        }
    }
}
