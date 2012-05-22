using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Messages;
using DotNetOpenAuth.OpenId.RelyingParty;
using NBlog.Views;
using TJ.CQRS.Messaging;
using TJ.Extensions;

namespace NBlog.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static OpenIdRelyingParty openIdRelyingParty = new OpenIdRelyingParty();
        private readonly IUserView _userView;
        private readonly IBlogView _blogView;
        private readonly ICommandBus _commandBus;
        private UrlHelper _urlHelper;

        public AuthenticationService(IUserView userView, IBlogView blogView, ICommandBus commandBus)
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
            var request = openIdRelyingParty.CreateRequest(identifier);
            request.AddExtension(GetClaim());
            return request.RedirectingResponse.AsActionResult();
        }

        private IOpenIdMessageExtension GetClaim()
        {
            return new ClaimsRequest()
                       {
                           Email = DemandLevel.Require,
                           FullName = DemandLevel.Require
                       };
        }

        public bool TryGetOpenIdResponse(out IAuthenticationResponse openIdResponse)
        {
            openIdResponse = openIdRelyingParty.GetResponse();
            if (openIdResponse.IsNull())
            {
                return false;
            }
            return true;
        }

        public OpenIdData ParseOpenIdResponse(IAuthenticationResponse openIdResponse)
        {
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

        public bool TryAuthenticateUser(string openId, out UserViewItem user)
        {
            user = _userView.GetUser(openId);
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
}