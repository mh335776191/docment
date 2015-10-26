using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Core.Ioc;
using Plugin.Core.Plugin;
using Autofac;
//using Sun.Core.Caching;

namespace Plugin.Framework
{
    public class Start
    {
        public static void Init()
        {
            WebIoc.Register(b=>b.RegisterInstance(FileOperate.Instance).As<IFileOperate>().ExternallyOwned());
            WebIoc.Register(b=>b.RegisterInstance(Loader.Instance).As<ILoader>().ExternallyOwned());
            WebIoc.Register(b=>b.RegisterInstance(Manage.Instance).As<IManage>().ExternallyOwned());
            WebIoc.Register(b => b.RegisterInstance(ZipManage.Instance).As<IZipManage>().ExternallyOwned());
            ZipManage.Instance.Initialize();
            Manage.Instance.Initialize();
        }
    }
}
