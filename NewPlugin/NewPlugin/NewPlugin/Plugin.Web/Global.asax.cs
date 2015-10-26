using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;
using System.Web.Optimization;
using Plugin.Core.Plugin;
using Plugin.Core.Ioc;
using Plugin.Framework;
using Autofac;
//using Plugin.Core.Caching;
//using Plugin.BaseOperate.Caching;

namespace Plugin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //WebIoc.Register(b => b.RegisterGeneric(typeof(ModelCacheFac<>)).As(typeof(IModelCacheFac<>)).SingleInstance());
            //MVC初始化
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            AreaRegistration.RegisterAllAreas();
            //初始化框架
            Start.Init();
            RouteTable.Routes.MapMvcAttributeRoutes();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
