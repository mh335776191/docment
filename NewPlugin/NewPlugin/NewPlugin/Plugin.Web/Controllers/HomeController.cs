 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using Plugin.BaseOperate.Interface;
using System.IO;
using Plugin.Core.Plugin;
using Plugin.Core.Ioc;
using Autofac;

namespace Plugin.Web.Controllers
{
    //[RouteArea("wo")]
    public class HomeController : Controller
    {
        //IModuleOp ModuleOp;
        //public HomeController(IModuleOp op)
        //{
        //    ModuleOp = op;
        //}

        // 我是Sun.Web的主页面
        
        public ActionResult Index(int id=0)
        {
            IManage manage = WebIoc.Container.Resolve<IManage>();
            ViewData["id"] = id;
            IEnumerable<PluginInfo> list =null;
            switch (id)
            {
                case 0: list = manage.GetPlugins().Where(p=>p.Status!= PluginStatus.Unload); break;
                case 1: list = manage.GetPlugins().Where(p => p.Status == PluginStatus.Usable); break;
                case 2: list = manage.GetPlugins().Where(p => p.Status == PluginStatus.Stop); break;
                default: list = manage.GetPlugins().Where(p => p.Status == PluginStatus.Error); break;
            }
            return View(list);
        }
        
        public ActionResult NavPart(int id)
        {
            return View(id);
        }
        public JsonResult Stop(string guid)
        {
			try
			{
				IManage manage = WebIoc.Container.Resolve<IManage>();
				manage.StopPlugin(guid);
				return Json(true);
			}
			catch(Exception e) {
				return Json(e.Message);
			}
            
        }
        public JsonResult Unload(string guid)
        {
			try
			{
				IManage manage = WebIoc.Container.Resolve<IManage>();
				manage.UnloadPlugin(guid);
				return Json(true);
			}
			catch (Exception e)
			{
				return Json(e.Message);
			}
		}
        public JsonResult Start(string guid)
        {
			try
			{
				IManage manage = WebIoc.Container.Resolve<IManage>();
				manage.StartPlugin(guid);
				return Json(true);
			}
			catch (Exception e)
			{
				return Json(e.Message);
			}
		}
        //安装包处理部分
        //[Route("home/Zips/{id?}")]
        public ActionResult Zips(int id = 0)
        {
            ViewData["id"] = id;
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Plugins/zips/"));
            var zips = dir.GetFiles("*.zip").OrderByDescending(f => f.CreationTime);
            return View(zips);
        }
        public JsonResult UploadPlugin()
        {
            var file = Request.Files[0];
            var FileName = file.FileName;
            var fileExt = Path.GetExtension(FileName);
            FileName = FileName.TrimEnd(fileExt.ToArray()) + fileExt.ToLower();
            if (fileExt.ToLower() != ".zip")
                return Json("插件包格式错误");
            var uploadFileBytes = new byte[file.ContentLength];
            var savePath = Server.MapPath("~/Plugins/zips/" + FileName);
            try
            {
                file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
                System.IO.File.WriteAllBytes(savePath, uploadFileBytes);
                IZipManage manage = WebIoc.Container.Resolve<IZipManage>();
                manage.RegisterZip(FileName);
            }
            catch (Exception e)
            {
                System.IO.File.Delete(savePath);
                return Json(e.Message);
            }
            return Json(true);
        }
        public JsonResult InstallZip(string name)
        {
            try
            {
                IZipManage manage = WebIoc.Container.Resolve<IZipManage>();
                manage.InstallZip(name);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
        /// <summary>
        /// 根据压缩包注册插件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult Register(string name)
        {
            try
            {
                IZipManage manage = WebIoc.Container.Resolve<IZipManage>();
                manage.RegisterZip(name);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
        public JsonResult DeleteZip(string name)
        {
            try
            {
                IZipManage manage = WebIoc.Container.Resolve<IZipManage>();
                manage.Delete(name);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}