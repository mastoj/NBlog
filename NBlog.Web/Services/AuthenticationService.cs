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

namespace NBlog.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static OpenIdRelyingParty openIdRelyingParty = new OpenIdRelyingParty();
        private readonly IUserView _userView;

        public AuthenticationService(IUserView userView)
        {
            _userView = userView;
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
            user = _userView.GetUserByAuthenticationId(openId);
            if(user == null)
            {
                return false;
            }
            return true;
        }
    }
}