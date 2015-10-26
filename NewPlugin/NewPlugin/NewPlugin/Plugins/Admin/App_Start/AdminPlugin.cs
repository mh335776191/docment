using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using Plugin.Core.Plugin;
using Plugin.Core.Ioc;
using Autofac;
using Plugin.Framework.MVC;
using Plugin.Admin.Controllers;

namespace Plugin.Admin
{
    [IocExport(typeof(IPlugin), ContractName = "2304e992-d781-4d04-8504-2d4e45c4e58f", SingleInstance = true)]
    public class AdminPlugin : IPlugin
    {
        public AdminPlugin()
        {

        }
        public string Name
        {
            get
            {
                return "Admin";
            }
        }
        PluginInfo pluginInfo;
        List<RouteBase> routeList = new List<RouteBase>();
        public void Initialize(PluginInfo info)
        {
            pluginInfo = info;
        }

        public void Stop()
        {
            IFileOperate fileOp = WebIoc.Container.Resolve<IFileOperate>();
            pluginInfo.Status = PluginStatus.Stop;
            fileOp.SavePluginInfo(pluginInfo);
        }

        public void Start()
        {
            IFileOperate fileOp = WebIoc.Container.Resolve<IFileOperate>();
            pluginInfo.Status = PluginStatus.Usable;
            fileOp.SavePluginInfo(pluginInfo);
        }
    }
}