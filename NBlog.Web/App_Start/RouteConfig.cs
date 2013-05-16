using System.Web.Mvc;
using System.Web.Routing;

namespace NBlog.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "FeedRoutes",
                url: "Feed/{Action}",
                defaults: new { controller = "Feed", action = "Index" },
                namespaces: new[] { "NBlog.Controllers" }
                );

            routes.MapRoute(
                name: "PostRoutes",
                url: "{slug}",
                defaults: new { controller = "Post", action = "Show" },
                constraints: new { slug = @".+" },
                namespaces: new[] { "NBlog.Controllers" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Post", action = "Index" },
                namespaces: new[] { "NBlog.Controllers" }
            );

            routes.MapRoute(
                name: "RedirectRoute",
                url: "{*oldUrl}",
                defaults: new { controller = "Post", action = "RedirectUrl" });
        }
    }
}