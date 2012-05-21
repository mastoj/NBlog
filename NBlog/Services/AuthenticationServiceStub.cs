using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NBlog.Views;

namespace NBlog.Services
{
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

        public bool TryAuthenticateUser(string openId, out UserViewItem user)
        {
            user = _userView.GetUser(openId);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}