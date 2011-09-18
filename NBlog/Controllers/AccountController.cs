using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasySec.Hashing;
using NBlog.Data;
using NBlog.Models;

namespace NBlog.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private IHashGenerator _hashGenerator;

        public AccountController(IUserRepository userRepository, IHashGenerator hashGenerator)
        {
            _userRepository = userRepository;
            _hashGenerator = hashGenerator;
        }

        //
        // GET: /Account/

        public ActionResult LogIn()
        {
            if (_userRepository.All().Count() == 0)
            {
                return RedirectToAction("CreateAdmin");
            }
            return View();
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
                var user = new User() {UserName = model.UserName};
                user.PasswordHash = _hashGenerator.GenerateHash(model.Password);
                _userRepository.Insert(user);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
