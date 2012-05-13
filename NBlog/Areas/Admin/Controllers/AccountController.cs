﻿using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NBlog.Domain.Commands;
using NBlog.Models;
using NBlog.Services;
using NBlog.Views;
using TJ.CQRS.Messaging;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISendCommand _commandBus;
        private readonly IAuthorView _authorView;
        //
        // GET: /Admin/Account/
        public AccountController(IAuthenticationService authenticationService, ISendCommand commandBus, IAuthorView authorView)
        {
            _authenticationService = authenticationService;
            _commandBus = commandBus;
            _authorView = authorView;
        }

        public virtual ActionResult Login(string returnUrl)
        {
            if (_authenticationService.IsUserAuthenticated(User))
            {
                return new RedirectResult(returnUrl);
            }
            var authenticationUrl = _authenticationService.GetAuthenticationUrl(returnUrl);
            return authenticationUrl;
        }

        public virtual ActionResult AuthenticateUser(string returnUrl)
        {
            var openIdData = _authenticationService.ParseOpenIdResponse();
            Author user;
            if (_authenticationService.TryAuthenticateUser(openIdData, out user))
            {
                SetAuthenticationCookie(user.AuthorId);
                return new RedirectResult(returnUrl);                
            }
            if (UserDoesNotExist())
            {

                var createUserComand = new CreateUserCommand()
                                           {
                                               OpenId = openIdData.OpenId,
                                               FriendlyName = openIdData.FriendlyName
                                           };
                return View(MVC.Admin.Account.Views.RegisterUser, createUserComand);
            }
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }

        private void SetAuthenticationCookie(string authorId)
        {
            bool isPersistent = false;
            int timeoutInMinutes = 60;
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(authorId, isPersistent, timeoutInMinutes);

            // add cookie to response stream
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        public virtual ActionResult RegisterUser(string returnUrl, CreateUserCommand createUserCommand)
        {
            if(ModelState.IsValid && UserDoesNotExist())
            {
                _commandBus.Send(createUserCommand);
                return new RedirectResult(returnUrl);
            }
            return View(createUserCommand);
        }

        private bool UserDoesNotExist()
        {
            return _authorView.GetAuthors().Count() == 0;
        }
    }
}
