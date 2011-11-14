using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EasySec.Hashing;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Helpers;
using NBlog.Infrastructure;
using NBlog.Models;
using NBlog.Translators;
using TJ.Extensions;
using TJ.Mvc.Filter;

namespace NBlog.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private IHashGenerator _hashGenerator;
        private IAuthenticationHandler _authenticationHandler;

        public AccountController(IUserRepository userRepository, IHashGenerator hashGenerator, IAuthenticationHandler authenticationHandler)
        {
            _userRepository = userRepository;
            _hashGenerator = hashGenerator;
            _authenticationHandler = authenticationHandler;
        }

        //
        // GET: /Account/

        public ActionResult Login()
        {
            if (UsersExist().IsFalse())
            {
                _authenticationHandler.LogoutUser();
                return RedirectToAction("CreateAdmin");
            }
            if (User.Identity.IsAuthenticated.IsTrue())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogInViewModel model)
        {
            if (ModelState.IsValid && ValidateCredentials(model))
            {
                return RedirectFromLogin();
            }
            return View(model);
        }

        private ActionResult RedirectFromLogin()
        {
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }

        private bool ValidateCredentials(LogInViewModel model)
        {
            var isAuthenticated = _authenticationHandler.AuthenticateUser(model.UserName, model.Password);
            if (isAuthenticated.IsFalse())
            {
                var errorMessage = "Invalid user credentials";
                ModelState.AddModelError("", errorMessage);
                TempData.AddErrorMessage(errorMessage);
            }
            return isAuthenticated;
        }

        public ActionResult Logout()
        {
            _authenticationHandler.LogoutUser();
            TempData.AddSuccessMessage("Successfully signed out");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateAdmin()
        {
            if (UsersExist())
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(CreateAdminModel model)
        {
            if (UsersExist().IsTrue())
            {
                return RedirectToAction("Login");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.PasswordHash = _hashGenerator.GenerateHash(model.Password);
                    var user = model.ToDTO();
                    _userRepository.Insert(user);
                    _authenticationHandler.LoginUser(model.UserName);
                    return RedirectFromLogin();
                }
                return View(model);
            }
        }

        private bool UsersExist()
        {
            return _userRepository.All().Count() > 0;
        }
    }
}
