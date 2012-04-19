using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using NBlog.Views;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class AccountController : Controller
    {
        private readonly IAuthorView _authorView;
        private readonly IBlogView _blogView;
        //
        // GET: /Admin/Account/
        private static OpenIdRelyingParty openIdRelyingParty = new OpenIdRelyingParty();

        public AccountController(IAuthorView authorView, IBlogView blogView)
        {
            _authorView = authorView;
            _blogView = blogView;
        }

        public virtual ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
            }
            var response = openIdRelyingParty.GetResponse();
            if (response == null)
            {
                var identifier = Identifier.Parse("google");
                var request = openIdRelyingParty.CreateRequest(identifier);
                return request.RedirectingResponse.AsActionResult();
            }

            //Let us check the response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    var openId = response.ClaimedIdentifier;
                    // check if user exist
                    var user = _authorView.GetAuthor(openId);
                    string userName = string.Empty;
                    if (user != null)
                    {
                        userName = user.AuthorName;
                    }
                    else if (user.IsNull() && _blogView.GetBlogs().Count() == 0)
                    {
                        userName = response.FriendlyIdentifierForDisplay;
                    }
                    else
                    {
                        return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
                    }
                    var authCookie = FormsAuthentication.GetAuthCookie(userName, false);
                    Response.SetCookie(authCookie);
                    return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);

                case AuthenticationStatus.Canceled:
                    ViewBag.Message = "Canceled at provider";
                    break;
                case AuthenticationStatus.Failed:
                    ViewBag.Message = response.Exception.Message;
                    break;
            }
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }

    }

}
