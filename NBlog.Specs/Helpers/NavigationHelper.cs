using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBlog.Specs.Helpers
{
    public static class NavigationHelper
    {
        private static Dictionary<string, string> _pages = new Dictionary<string, string>()
                                                               {
                                                                   {"start page", ""},
                                                                   {"login page", "login"},
                                                                   {"admin page", "Admin/Home"},
                                                                   {"create admin page", "Account/CreateAdmin"}
                                                               };

        private static Dictionary<string, string> _links = new Dictionary<string, string>()
                                                               {
                                                                   {"log off", "logOff"}
                                                               };

        public static Dictionary<string, string> Pages
        {
            get { return _pages; }
        }

        public static Dictionary<string, string> Links
        {
            get { return _links; }
        }
    }
}
