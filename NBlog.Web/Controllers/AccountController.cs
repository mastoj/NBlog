using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.OpenId.RelyingParty;
using NBlog.Domain.Commands;
using NBlog.Views;
using NBlog.Web.Services;
using TJ.CQRS.Messaging;

namespace NBlog.Web.Controllers
{
    public partial class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICommandBus _commandBus;
        private readonly IUserView _userView;
        //
        // GET: /Admin/Account/
        public AccountController(IAuthenticationService authenticationService, ICommandBus commandBus, IUserView userView)
        {
            _authenticationService = authenticationService;
            _commandBus = commandBus;
            _userView = userView;
        }

        public virtual ActionResult Login(string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Post");
            if (_authenticationService.IsUserAuthenticated(User))
            {
                return new RedirectResult(returnUrl);
            }
            IAuthenticationResponse openIdResponse;
            var gotOpenIdResponse = _authenticationService.TryGetOpenIdResponse(out openIdResponse);
            if(gotOpenIdResponse)
            {
                return AuthenticateUser(returnUrl, openIdResponse);
            }
            var authenticationUrl = _authenticationService.GetAuthenticationUrl(returnUrl);
            return authenticationUrl;
        }

        private ActionResult AuthenticateUser(string returnUrl, IAuthenticationResponse openIdResponse)
        {
            var openIdData = _authenticationService.ParseOpenIdResponse(openIdResponse);
            UserViewItem user;
            var authenticationId = openIdData.OpenId;
            if (_authenticationService.TryAuthenticateUser(authenticationId, out user))
            {
                SetAuthenticationCookie(user.AuthenticationId.ToString());
                return new RedirectResult(returnUrl);                
            }
            if (UsersExist() == false)
            {
                return RedirectToAction("RegisterUser", new {UserId = openIdData.OpenId, returnUrl = returnUrl});
            }
            return RedirectToAction("Index", "Post");
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

        public ActionResult RegisterUser(string UserId)
        {
            var createUserComand = new CreateUserCommand()
            {
                AuthenticationId = UserId
            };
            return View("RegisterUser", createUserComand);

        }

        [HttpPost]
        public virtual ActionResult RegisterUser(string returnUrl, CreateUserCommand createUserCommand)
        {
            if(ModelState.IsValid && UsersExist() == false)
            {
                _commandBus.Send(createUserCommand);
                SetAuthenticationCookie(createUserCommand.AuthenticationId);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            return View(createUserCommand);
        }

        public ActionResult ToggleAdminMode(string returnUrl)
        {
            var modeCookie = Request.Cookies.Get("CurrentMode");
            if(modeCookie == null)
            {
                modeCookie = new HttpCookie("CurrentMode", "admin");
            }
            return Redirect(returnUrl);
        }

        private bool UsersExist()
        {
            return _userView.GetUsers().Count() > 0;
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Post");
        }
    }
}
