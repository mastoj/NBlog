using System;
using System.Collections.Generic;
using System.Linq;

namespace NBlog.Infrastructure
{
    public interface IAuthenticationManager
    {
        void LoginUser(string userName);
        void LogoutUser();
    }
}