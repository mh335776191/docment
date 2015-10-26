using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Plugin.Framework.MVC;

namespace Plugin.Admin
{
    public class RouteConfig : MVCRouteConfig
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            var route1 = RouteTable.Routes.MapRoute(
                name: "admin",
                url: "admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional,areaname = "admin" }
            );
        }
    }
}