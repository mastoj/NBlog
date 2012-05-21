using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using NBlog.Models;
using NBlog.Views;

namespace NBlog.Services
{
    public interface IAuthenticationService
    {
        bool IsUserAuthenticated(IPrincipal user);
        ActionResult GetAuthenticationUrl(string returnUrl);
        OpenIdData ParseOpenIdResponse();
        bool TryAuthenticateUser(string openId, out UserViewItem user);
    }

    public class OpenIdData
    {
        public string OpenId { get; set; }
        public string FriendlyName { get; set; }
    }
}