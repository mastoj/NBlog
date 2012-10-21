using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Messages;
using DotNetOpenAuth.OpenId.RelyingParty;
using NBlog.Views;

namespace NBlog.Web.Services
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
                        action = "Login",
                        controller = "Account",
                        returnUrl = returnUrl
                    }
                );
            return new RedirectToRouteResult(routeValues); 
        }

        public OpenIdData ParseOpenIdResponse(IAuthenticationResponse openIdResponse)
        {
            return new OpenIdData()
                       {
                           FriendlyName = "Tomas Jansson",
                           OpenId = _openIdGuid.ToString()
                       };
        }

        public bool TryAuthenticateUser(string authenticationId, out UserViewItem user)
        {
            user = _userView.GetUserByAuthenticationId(authenticationId);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public bool TryGetOpenIdResponse(out IAuthenticationResponse openIdResponse)
        {
            openIdResponse = new AuthenticationResponseStub();
            return true;
        }
    }

    public class AuthenticationResponseStub : IAuthenticationResponse
    {
        public string GetCallbackArgument(string key)
        {
            throw new NotImplementedException();
        }

        public string GetUntrustedCallbackArgument(string key)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetCallbackArguments()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetUntrustedCallbackArguments()
        {
            throw new NotImplementedException();
        }

        public T GetExtension<T>() where T : IOpenIdMessageExtension
        {
            throw new NotImplementedException();
        }

        public IOpenIdMessageExtension GetExtension(Type extensionType)
        {
            throw new NotImplementedException();
        }

        public T GetUntrustedExtension<T>() where T : IOpenIdMessageExtension
        {
            throw new NotImplementedException();
        }

        public IOpenIdMessageExtension GetUntrustedExtension(Type extensionType)
        {
            throw new NotImplementedException();
        }

        public Identifier ClaimedIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string FriendlyIdentifierForDisplay
        {
            get { throw new NotImplementedException(); }
        }

        public AuthenticationStatus Status
        {
            get { throw new NotImplementedException(); }
        }

        public IProviderEndpoint Provider
        {
            get { throw new NotImplementedException(); }
        }

        public Exception Exception
        {
            get { throw new NotImplementedException(); }
        }
    }
}