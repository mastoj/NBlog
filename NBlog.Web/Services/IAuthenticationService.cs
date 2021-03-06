﻿using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using NBlog.Views;

namespace NBlog.Web.Services
{
    public interface IAuthenticationService
    {
        bool IsUserAuthenticated(IPrincipal user);
        ActionResult GetAuthenticationUrl(string returnUrl);
        OpenIdData ParseOpenIdResponse(IAuthenticationResponse openIdResponse);
        bool TryAuthenticateUser(string authenticationId, out UserViewItem user);
        bool TryGetOpenIdResponse(out IAuthenticationResponse openIdResponse);
        UserMode GetUserMode(HttpRequestBase request);
        void SetUserMode(HttpResponseBase response, UserMode userMode);
    }

    public class OpenIdData
    {
        public string OpenId { get; set; }
        public string FriendlyName { get; set; }
    }
}