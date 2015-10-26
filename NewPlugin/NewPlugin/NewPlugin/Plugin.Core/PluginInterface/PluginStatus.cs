using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Core.Plugin
{
    public enum PluginStatus
    {
        /// <summary>
        /// 插件错误
        /// </summary>
        Error = 0,
        /// <summary>
        /// 插件可用
        /// </summary>
        Usable = 1,
        /// <summary>
        /// 已停用
        /// </summary>
        Stop = 2,
        /// <summary>
        /// 插件需要卸载
        /// </summary>
        Unload=3
    }
}
