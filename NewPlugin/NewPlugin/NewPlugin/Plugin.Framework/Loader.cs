﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Plugin.Core.Plugin;
using Plugin.Core.Ioc;
using Plugin.Framework.Zip;
using Plugin.Framework.File;

namespace Plugin.Framework
{
    public class Loader : ILoader
    {
        IFileOperate fileParser = FileOperate.Instance;
        static Loader _instance;
        public static Loader Instance
        {
            get
            {
                if (null == _instance)
                    _instance = new Loader();
                return _instance;
            }
        }
        #region Const
        /// <summary>
        /// 插件路径
        /// </summary>
        private static string pluginBasePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Plugins");
        /// <summary>
        /// 插件临时目录
        /// </summary>
        private static string tempBasePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"App_Data\Plugins");

        /// <summary>
        /// 插件自述文件名
        /// </summary>
        public const string pluginInfoFileName = "PluginInfo.json";

        public string PluginBasePath
        {
            get { return pluginBasePath; }
        }

        public string TempBasePath
        {
            get { return tempBasePath; }
        }

        public string PluginInfoFileName
        {
            get { return pluginInfoFileName; }
        }

        #endregion

        /// <summary>
        /// 插件目录。
        /// </summary>
        private static DirectoryInfo PluginBaseFolder;

        /// <summary>
        /// 插件临时目录。
        /// </summary>
        private static DirectoryInfo TempPluginFolder;
        /// <summary>
        /// 存储bin目录下面所有文件名
        /// </summary>
        private static List<string> FrameworkPrivateBinFiles;

        /// <summary>
        /// 初始化。
        /// </summary>
        static Loader()
        {
            PluginBaseFolder = new DirectoryInfo(pluginBasePath);//创建插件目录对象
            TempPluginFolder = new DirectoryInfo(tempBasePath);//创建插件临时目录对象
            FrameworkPrivateBinFiles = new DirectoryInfo(System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath).GetFiles().Select(p => p.Name).ToList();
        }

        /// <summary>
        /// 加载插件。
        /// </summary>
        public IEnumerable<PluginInfo> Load()
        {
            List<PluginInfo> plugins = new List<PluginInfo>();
            var plugininfoFiles = PluginBaseFolder.GetFiles(pluginInfoFileName, SearchOption.AllDirectories);//获取所有插件的配置文件
            foreach (var pluginInfoFolder in plugininfoFiles)//获取插件目录的所有子目录
            {
                //解析插件自述信息
                try
                {
                    var pluginInfo = fileParser.ParsePluginInfo(pluginInfoFolder);
                    pluginInfo.DirectoryName = pluginInfoFolder.Directory.Name;
                    pluginInfo.Path = pluginInfoFolder.FullName.Substring(0, pluginInfoFolder.FullName.Length - pluginInfoFileName.Length);
                    if (pluginInfo.Status == PluginStatus.Unload)
                    {//如果插件已经卸载，则删除该插件的所有文件
                        DirectoryInfo dir = new DirectoryInfo(pluginInfo.Path);
                        dir.Delete(true);
                        continue;
                    }
                    plugins.Add(pluginInfo);
                }
                catch
                {

                }
            }
            plugins.OrderBy(p => p.SortIndex);//通过展现顺序对插件列表进行排序

            //程序集复制到临时目录。
            CopyToTempPluginFolderDirectory(plugins);
            return plugins;
        }
        /// <summary>
        /// 从ZIP文件中加载插件
        /// </summary>
        /// <param name="zipPath">zip文件地址</param>
        /// <returns></returns>
        public PluginInfo LoadFromZip(string zipPath,bool load=true)
        {
            PluginInfo pluginInfo = null;
            var name = Path.GetFileName(zipPath);
            name = name.Substring(0, name.Length - 4);
            var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "zipTemp",name);
            ZipHelper.Extract(zipPath, tempPath);
            var tempDir = new DirectoryInfo(tempPath);
            var files = tempDir.GetFiles(pluginInfoFileName, SearchOption.AllDirectories);
            try
            {
                if (files.Length > 0)
                {
                    pluginInfo = fileParser.ParsePluginInfo(files[0]);
                    if (load)
                    {
                        string pluginPath = null;
                        if (!string.IsNullOrEmpty(pluginInfo.GroupName))
                            pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", pluginInfo.Type.ToString(), pluginInfo.GroupName, name);
                        else
                            pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", pluginInfo.Type.ToString(), name);
                        var pluginDir = new DirectoryInfo(pluginPath);
                        if (!pluginDir.Exists)
                        {
                            pluginDir.Create();
                        }
                        FileHelper.CopyDirectory(tempPath, pluginPath);
                        pluginInfo.Path = pluginPath;
                        CopyToTempPluginFolderDirectory(new List<PluginInfo>() { pluginInfo }, false);
                    }
                }
                else
                {
                    throw new Exception("插件缺少" + PluginInfoFileName + "文件");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally {
                tempDir.Delete(true);//删除临时文件
            }
            return pluginInfo;
        }

        /// <summary>
        /// 程序集复制到临时目录。
        /// </summary>
        private void CopyToTempPluginFolderDirectory(List<PluginInfo> pluginInfos,bool clearTemp=true)
        {
            //临时目录如果不存在则创建
            if (TempPluginFolder.Exists)
            {
                if (clearTemp)
                {
                    TempPluginFolder.Delete(true);
                    TempPluginFolder.Create();
                }
            }
            else
            {
                TempPluginFolder.Create();
            }
            
            //复制插件进临时文件夹。
            foreach (var plugin in pluginInfos)
            {
                var dir = new DirectoryInfo(plugin.Path);//获取插件bin目录对象
                var fileList = dir.GetFiles("*.dll", SearchOption.AllDirectories);
                //插件有用程序集文件列表
                var plugindlls = new List<FileInfo>();
                foreach (var file in fileList)//遍历插件里面的所有dll文件
                {
                    if (FrameworkPrivateBinFiles.Contains(file.Name) == true)//如果该dll组建已经存在主程序bin目录下则跳过
                    {
                        // file.Delete();
                        continue;
                    }
                    if (plugin.AssemblyNames.Contains(file.Name) == true&&!plugindlls.Any(f=>f.Name==file.Name))
                    {
                        plugindlls.Add(file);
                    }
                    else
                    {
                        // file.Delete();
                    }
                }
                foreach (var plugindll in plugindlls)//遍历插件有用程序集列表
                {
                    var srcPath = plugindll.FullName;
                    var toPath = Path.Combine(TempPluginFolder.FullName, plugindll.Name);
                    System.IO.File.Copy(srcPath, toPath, true);//拷贝程序集到临时目录
                    PluginIoc.Register(b => b.RegisterTypeFromFile(toPath));
                }
            }
            if (null == PluginIoc.Container)
                PluginIoc.Build();
            else
                PluginIoc.Rebuild();
        }
    }
}
