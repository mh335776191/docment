using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Plugin.Framework.MVC
{
    /// <summary>
    /// MVC路由配置
    /// </summary>
    public abstract class MVCRouteConfig
    {
        /// <summary>
        /// 注册路由信息
        /// </summary>
        /// <param name="routes"></param>
        public abstract void RegisterRoutes(RouteCollection routes);
    }
}
