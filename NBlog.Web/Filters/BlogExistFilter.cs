using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NBlog.Views;
using TJ.Extensions;

namespace NBlog.Web.Filters
{
    public class BlogExistFilter : ActionFilterAttribute
    {
        private readonly IBlogView _blogView;
        private static bool _blogIsCreated;

        public BlogExistFilter(IBlogView blogView)
        {
            _blogView = blogView;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (BlogIsCreated().IsFalse() && IsCreateBlogRequest(filterContext).IsFalse()
                && IsAuthenticationResquest(filterContext).IsFalse()
                && filterContext.IsChildAction.IsFalse())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new Dictionary<string, object>()
                            {
                                { "controller", "Blog" },
                                { "action", "Create" }
                            }
                    ));
            }
        }

        private bool IsAuthenticationResquest(ActionExecutingContext filterContext)
        {
            var routeData = filterContext.RouteData;
            var controller = routeData.Values["controller"];
            if (controller.Equals("Account"))
            {
                return true;
            }
            return false;
        }

        private bool IsCreateBlogRequest(ActionExecutingContext filterContext)
        {
            var routeData = filterContext.RouteData;
            var controller = routeData.Values["controller"];
            var action = routeData.Values["action"];
            if (controller.Equals("Blog") && action.Equals("Create"))
            {
                return true;
            }
            return false;
        }

        private bool BlogIsCreated()
        {
            if (_blogIsCreated.IsFalse())
            {
                var blog = _blogView.GetBlogs().FirstOrDefault();
                _blogIsCreated = blog.IsNotNull();
            }
            return _blogIsCreated;
        }
    }
}
