using System;
using System.Collections.Generic;
using System.Linq;

namespace NBlog.Infrastructure
{
    public interface IAuthenticationManager
    {
        void SignInUser(string userName);
        void SignOutUser();
    }
}