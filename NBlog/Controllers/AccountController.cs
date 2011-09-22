using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EasySec.Hashing;
using NBlog.Data;
using NBlog.Helpers;
using NBlog.Infrastructure;
using NBlog.Models;
using TJ.Extensions;

namespace NBlog.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private IHashGenerator _hashGenerator;
        private IAuthenticationManager _authenticationManager;

        public AccountController(IUserRepository userRepository, IHashGenerator hashGenerator, IAuthenticationManager authenticationManager)
        {
            _userRepository = userRepository;
            _hashGenerator = hashGenerator;
            _authenticationManager = authenticationManager;
        }

        //
        // GET: /Account/

        public ActionResult LogIn()
        {
            if (_userRepository.All().Count() == 0)
            {
                SignOutUserIfSignedIn();
                return RedirectToAction("CreateAdmin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid && ValidateCredentials(model))
            {
                _authenticationManager.SignInUser(model.UserName);
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View(model);
        }

        private bool ValidateCredentials(LogInViewModel model)
        {
            var user = _userRepository.Single(y => y.UserName == model.UserName);
            var isValid = user.IsNotNull() && _hashGenerator.CompareHash(user.PasswordHash, model.Password);
            if (isValid.IsFalse())
            {
                var errorMessage = "Invalid user credentials";
                ModelState.AddModelError("", errorMessage);
                TempData.AddErrorMessage(errorMessage);
            }
            return isValid;
        }

        private void SignOutUserIfSignedIn()
        {
            if (User.IsNotNull() && User.Identity.IsNotNull() && User.Identity.IsAuthenticated.IsTrue())
            {
                _authenticationManager.SignOutUser();
            }
        }

        public ActionResult LogOff()
        {
            throw new NotImplementedException();
        }

        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(CreateAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User() { UserName = model.UserName, Name = model.Name };
                user.PasswordHash = _hashGenerator.GenerateHash(model.Password);
                _userRepository.Insert(user);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
