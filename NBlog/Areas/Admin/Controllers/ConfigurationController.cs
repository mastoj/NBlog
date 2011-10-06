using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBlog.Areas.Admin.Controllers
{
    public class ConfigurationController : Controller
    {
        //
        // GET: /Admin/Configuration/

        public ActionResult Index()
        {
            return View();
        }

    }
}
