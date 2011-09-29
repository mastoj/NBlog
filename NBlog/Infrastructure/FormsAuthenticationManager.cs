using System.Web;
using System.Web.Security;
using TJ.Extensions;

namespace NBlog.Infrastructure
{
    public class FormsAuthenticationManager : IAuthenticationManager
    {
        public void LoginUser(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, false);
        }

        public void LogoutUser()
        {
            var user = HttpContext.Current.User;
            if (user.IsNotNull() && user.Identity.IsNotNull() && user.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}