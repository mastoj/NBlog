using System;
using System.Collections.Generic;
using System.Linq;

namespace NBlog.Infrastructure
{
    public interface IAuthenticationHandler
    {
        void LoginUser(string userName);
        void LogoutUser();
        bool AuthenticateUser(string userName, string password);
    }
}