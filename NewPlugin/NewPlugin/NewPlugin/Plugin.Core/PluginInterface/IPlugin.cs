using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Core.Plugin
{
    public interface IPlugin
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 初始化。
        /// </summary>
        void Initialize(PluginInfo info);
        /// <summary>
        /// 启用插件
        /// </summary>
        void Start();
        /// <summary>
        /// 停用插件
        /// </summary>
        void Stop();
    }
}
