using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Messages;
using DotNetOpenAuth.OpenId.RelyingParty;
using NBlog.Models;
using NBlog.Views;
using TJ.CQRS.Messaging;
using TJ.Extensions;

namespace NBlog.Services
{
    public interface IAuthenticationService
    {
        bool IsUserAuthenticated(IPrincipal user);
        ActionResult GetAuthenticationUrl(string returnUrl);
        OpenIdData ParseOpenIdResponse();
        bool TryAuthenticateUser(OpenIdData openIdData, out UserViewItem user);
    }

    public class AuthenticationServiceStub : IAuthenticationService
    {
        private readonly IUserView _userView;
        private UrlHelper _urlHelper;
        private static Guid _openIdGuid = Guid.Empty;

        public AuthenticationServiceStub(IUserView userView)
        {
            _userView = userView;
            HttpContextWrapper httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
            _urlHelper = new UrlHelper(new RequestContext(httpContextWrapper, RouteTable.Routes.GetRouteData(httpContextWrapper)));      
        }

        public bool IsUserAuthenticated(IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public ActionResult GetAuthenticationUrl(string returnUrl)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary(
                new {
                        action = MVC.Admin.Account.ActionNames.AuthenticateUser,
                        controller = MVC.Admin.Account.Name,
                        returnUrl = returnUrl
                    }
                );
            return new RedirectToRouteResult(routeValues); 
        }

        public OpenIdData ParseOpenIdResponse()
        {
            return new OpenIdData()
                       {
                           FriendlyName = "Tomas Jansson",
                           OpenId = _openIdGuid.ToString()
                       };
        }

        public bool TryAuthenticateUser(OpenIdData openIdData, out UserViewItem user)
        {
            user = _userView.GetUser(openIdData.OpenId);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }

    public class AuthenticationService : IAuthenticationService
	{
        private static OpenIdRelyingParty openIdRelyingParty = new OpenIdRelyingParty();
        private readonly IUserView _userView;
        private readonly IBlogView _blogView;
        private readonly ISendCommand _commandBus;
        private UrlHelper _urlHelper;

        public AuthenticationService(IUserView userView, IBlogView blogView, ISendCommand commandBus)
        {
            _userView = userView;
            _blogView = blogView;
            _commandBus = commandBus;
            HttpContextWrapper httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
            _urlHelper = new UrlHelper(new RequestContext(httpContextWrapper, RouteTable.Routes.GetRouteData(httpContextWrapper)));
        }

        public bool IsUserAuthenticated(IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public ActionResult GetAuthenticationUrl(string returnUrl)
        {
            var identifier = Identifier.Parse("https://www.google.com/accounts/o8/id");
            var routeDictionary =
                    new
                        {
                            action = MVC.Admin.Account.ActionNames.AuthenticateUser,
                            controller = MVC.Admin.Account.Name,
                            returnUrl = returnUrl
                        };
            Realm realm = GetRealm();
            var request = openIdRelyingParty.CreateRequest(identifier, realm, new Uri(_urlHelper.RouteUrl("Admin_default", routeDictionary, "http")));
            request.AddExtension(GetClaim());
            return request.RedirectingResponse.AsActionResult();
        }

        private Realm GetRealm()
        {
            var realmUrl = _urlHelper.RouteUrl("Account_route", new {action = "Login", controller = "Account"}, "http");
            var realm = new Realm(realmUrl);
            return realm;
        }

        private IOpenIdMessageExtension GetClaim()
        {
            return new ClaimsRequest()
                       {
                           Email = DemandLevel.Require,
                           FullName = DemandLevel.Require
                       };
        }

        public OpenIdData ParseOpenIdResponse()
        {
            var openIdResponse = openIdRelyingParty.GetResponse();
            if (openIdResponse.IsNull())
            {
                throw new ArgumentException("Invalid open id response");
            }
            //Let us check the response
            switch (openIdResponse.Status)
            {
                case AuthenticationStatus.Authenticated:
                    var openId = openIdResponse.ClaimedIdentifier;
                    var friendlyName = openIdResponse.FriendlyIdentifierForDisplay;
                    return new OpenIdData() {FriendlyName = friendlyName, OpenId = openId};
                    break;
                default:
                    throw new ArgumentException("Invalid open id");
            }

        }

        public bool TryAuthenticateUser(OpenIdData openIdData, out UserViewItem user)
        {
            user = _userView.GetUser(openIdData.OpenId);
            if(user == null)
            {
                return false;
            }
            return true;
        }

        //public ActionResult RegisterUser(string returnUrl)
        //{
        //    var openIdResponse = openIdRelyingParty.GetResponse();
        //    if (openIdResponse.IsNull())
        //    {
        //        throw new ArgumentException("Invalid open id response");
        //    }
        //    //Let us check the response
        //    switch (openIdResponse.Status)
        //    {
        //        case AuthenticationStatus.Authenticated:
        //            var openId = openIdResponse.ClaimedIdentifier;
        //            // check if user exist
        //            var user = _userView.GetUser(openId);
        //            string userName = string.Empty;
        //            if (user != null)
        //            {
        //                userName = user.UserName;
        //            }
        //            else if (user.IsNull() && _blogView.GetBlogs().Count() == 0)
        //            {
        //                userName = openIdResponse.FriendlyIdentifierForDisplay;
        //            }
        //            else
        //            {
        //                return new RedirectResult(MVC.Home.Url.Action(MVC.Home.ActionNames.Index));
        //            }

        //            if (string.IsNullOrEmpty(returnUrl))
        //            {
        //                return new RedirectResult(MVC.Home.Url.Action(MVC.Home.ActionNames.Index));
        //            }
        //            return new RedirectResult(returnUrl);

        //        //case AuthenticationStatus.Canceled:
        //        //    message = "Canceled at provider";
        //        //    break;
        //        //case AuthenticationStatus.Failed:
        //        //    message = openIdResponse.Exception.Message;
        //        //    break;
        //    }
        //}
	}

    public class OpenIdData
    {
        public string OpenId { get; set; }
        public string FriendlyName { get; set; }
    }
}