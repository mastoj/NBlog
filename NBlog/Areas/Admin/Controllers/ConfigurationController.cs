using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class ConfigurationController : Controller
    {
        //
        // GET: /Admin/Configuration/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
