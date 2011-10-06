using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.Mvc.Filter;

namespace NBlog.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View("Index");
        }

    }
}
