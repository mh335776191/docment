using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plugin.Admin.Areas.News.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /News/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}