using System.Web.Optimization;

namespace Plugin.Web
{
    /// <summary>
    /// 静态文件加载
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// 静态文件加载
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/Scripts/sunbase").Include(
                        "~/Content/Scripts/jquery-1.11.2.min.js", "~/Content/Scripts/laytpl.js", "~/Content/Scripts/library.js"));
            bundles.Add(new ScriptBundle("~/Content/Scripts/easyuiext/").Include("~/Content/Scripts/easyui/locale/easyui-lang-zh_CN.js", "~/Content/Scripts/easyui/validateExt.js"));
            bundles.Add(new StyleBundle("~/Content/Scripts/easyui/themes/default/style").Include("~/Content/Scripts/easyui/themes/default/easyui.css"));
        }
    }
}