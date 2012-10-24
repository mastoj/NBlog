using System;
using System.Web.Http;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;

namespace NBlog.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //            HtmlHelper.ClientValidationEnabled = true;
            //            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            FluentValidationModelValidatorProvider.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var exception = Server.GetLastError();
                new LogEvent(exception.Message + ": " + exception.StackTrace).Raise();
            }
            catch (Exception)
            {
            }
        }
    }

    class LogEvent : WebRequestErrorEvent
    {
        public LogEvent(string message)
            : base(null, null, 100001, new Exception(message))
        {
        }
    }
}