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

        public ActionResult Login()
        {
            if (UsersExist().IsFalse())
            {
                _authenticationManager.LogoutUser();
                return RedirectToAction("CreateAdmin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogInViewModel model)
        {
            if (ModelState.IsValid && ValidateCredentials(model))
            {
                _authenticationManager.LoginUser(model.UserName);
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

        public ActionResult Logout()
        {
            _authenticationManager.LogoutUser();
            TempData.AddInfoMessage("Successfully signed out");
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
                    var user = new User() {UserName = model.UserName, Name = model.Name};
                    user.PasswordHash = _hashGenerator.GenerateHash(model.Password);
                    _userRepository.Insert(user);
                    _authenticationManager.LoginUser(user.UserName);
                    return RedirectToAction("Index", "Home", new {area = "Admin"});
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
