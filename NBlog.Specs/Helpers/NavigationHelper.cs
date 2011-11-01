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
                                                                   {"start page", "/"},
                                                                   {"login page", "/login"},
                                                                   {"admin page", "/Admin"},
                                                                   {"create admin page", "/Account/CreateAdmin"},
                                                                   {"create post page", "/Admin/Post/Create"},
                                                                   {"edit post page", "/Admin/Post/Edit/"}
                                                               };

        private static Dictionary<string, string> _links = new Dictionary<string, string>()
                                                               {
                                                                   {"log off", "logOff"},
                                                                   {"pages", "pages"},
                                                                   {"posts", "posts"},
                                                                   {"comments", "comments"},
                                                                   {"configuration", "configuration"}
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
