//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using System.Web.Routing;
//using NBlog.Views;
//using TJ.Extensions;

//namespace NBlog.Web.Filters
//{
//    public class BlogExistFilter : ActionFilterAttribute
//    {
//        private readonly IBlogView _blogView;

//        public BlogExistFilter(IBlogView blogView)
//        {
//            _blogView = blogView;
//        }

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            if(BlogIsCreated().IsFalse() && IsCreateBlogRequest(filterContext).IsFalse() 
//                && IsAuthenticationResquest(filterContext).IsFalse()
//                && filterContext.IsChildAction.IsFalse())
//            {
//                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
//                        new Dictionary<string, object>()
//                            {
//                                { "controller", MVC.Admin.Blog.Name },
//                                { "action", MVC.Admin.Blog.ActionNames.Create },
//                                { "area", MVC.Admin.Name }
//                            }
//                    ));
//            }
//        }

//        private bool IsAuthenticationResquest(ActionExecutingContext filterContext)
//        {
//            var routeData = filterContext.RouteData;
//            var controller = routeData.Values["controller"];
//            if (controller.Equals(MVC.Admin.Account.Name))
//            {
//                return true;
//            }
//            return false;
//        }

//        private bool IsCreateBlogRequest(ActionExecutingContext filterContext)
//        {
//            var routeData = filterContext.RouteData;
//            var controller = routeData.Values["controller"];
//            var action = routeData.Values["action"];
//            if(controller.Equals(MVC.Admin.Blog.Name) && action.Equals(MVC.Admin.Blog.ActionNames.Create))
//            {
//                return true;
//            }
//            return false;
//        }

//        private bool BlogIsCreated()
//        {
//            var blog = _blogView.GetBlogs().FirstOrDefault();
//            return blog.IsNotNull();
//        }
//    }
//}
