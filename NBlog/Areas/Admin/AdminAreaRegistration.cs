using System.Web.Mvc;

namespace NBlog.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Post_route",
                "Admin/Post/{action}/{id}",
                new { controller = MVC.Admin.Post.Name, action = MVC.Admin.Post.ActionNames.Index, id = UrlParameter.Optional },
                null,
                new[] { "NBlog.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Account_route",
                "Admin/{controller}/{action}/{id}",
                new { action = "Login", id = UrlParameter.Optional },
                null,
                new[] { "NBlog.Areas.Admin.Controllers" }
            );
        }
    }
}
