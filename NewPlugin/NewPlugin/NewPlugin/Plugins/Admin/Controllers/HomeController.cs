using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Sun.BaseOperate.Interface;
//using Plugin.Core.Caching;
using Plugin.Core.Ioc;
using Plugin.Core.Plugin;
using Autofac;

namespace Plugin.Admin.Controllers
{
    public class HomeController : Controller
    {
        //IModuleOp ModuleOp;
        //public HomeController(IModuleOp op)
        //{
        //    ModuleOp = op;
        //}
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //ICache cache = WebIoc.Container.ResolveNamed<ICache>("datas");
            return View();
        }
	}
}