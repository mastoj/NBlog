using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.OpenId.RelyingParty;
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
            if (_authenticationService.TryAuthenticateUser(openIdData.OpenId, out user))
            {
                SetAuthenticationCookie(user.UserId);
                return new RedirectResult(returnUrl);                
            }
            if (UserDoesNotExist())
            {
                var createUserComand = new CreateUserCommand()
                                           {
                                               UserId = openIdData.OpenId
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

        [HttpPost]
        public virtual ActionResult RegisterUser(string returnUrl, CreateUserCommand createUserCommand)
        {
            if(ModelState.IsValid && UserDoesNotExist())
            {
                _commandBus.Send(createUserCommand);
                UserViewItem user;
                if (_authenticationService.TryAuthenticateUser(createUserCommand.UserId, out user))
                {
                    SetAuthenticationCookie(user.UserId);
                    if(string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name, new { area = MVC.Home.Area });
                    }
                    return new RedirectResult(returnUrl);
                }
            }
            return View(createUserCommand);
        }

        private bool UserDoesNotExist()
        {
            return _userView.GetUsers().Count() == 0;
        }
    }
}
