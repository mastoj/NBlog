using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBlog.Areas.Admin.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Admin/Comment/

        public ActionResult Index()
        {
            return View();
        }

    }
}
