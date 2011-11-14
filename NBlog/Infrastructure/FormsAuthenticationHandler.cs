using System.Web;
using System.Web.Security;
using EasySec.Hashing;
using NBlog.Domain.Repositories;
using TJ.Extensions;

namespace NBlog.Infrastructure
{
    public class FormsAuthenticationHandler : IAuthenticationHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashGenerator _hashGenerator;

        public FormsAuthenticationHandler(IUserRepository userRepository, IHashGenerator hashGenerator)
        {
            _userRepository = userRepository;
            _hashGenerator = hashGenerator;
        }

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

        public bool AuthenticateUser(string userName, string password)
        {
            var user = _userRepository.Single(y => y.UserName == userName);
            var isValid = user.IsNotNull() && _hashGenerator.CompareHash(user.PasswordHash, password);
            if (isValid.IsTrue())
            {
                LoginUser(userName);
            }
            return isValid;
        }
    }
}