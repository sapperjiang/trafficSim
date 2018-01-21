using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;

namespace TrafficSim
{ 
    //internal sealed class InterfacesTypes
    //{
   
    //}
    class PluginHandler
    {
        internal static readonly string ICommand = "TranMACASims.ICommand";
        internal static readonly string ITool = "TranMACASims.ITool";
        internal static readonly string IMenuDef = "TranMACASims.IMenuDef";
        internal static readonly string IToolBarDef = "TranMACASims.IToolBarDef";
        internal static readonly string IDockableWindowDef = "TranMACASims.IDockableWindowDef";


        private static void getPluginObject(PluginContainer pluginCtner, Type _type)
        {
            IPlugin _plugin = null;

            try
            {
                _plugin = Activator.CreateInstance(_type) as IPlugin;

            }
            finally
            {
                if(_plugin != null)
                {
                    if(!pluginCtner.Contains(_plugin))
                    {
                        pluginCtner.Add(_plugin);
                    }
                }
            }

        }

        private static readonly string pluginFolder = System.Windows.Forms.Application.StartupPath + "\\plugin";
        internal static PluginContainer getPlugInsFromDll()
        {
            PluginContainer _pluginCtner = new PluginContainer();

            if (!Directory.Exists(pluginFolder))
            {
                Directory.CreateDirectory(pluginFolder);
            }

            string[] _files = Directory.GetFiles(pluginFolder, "*.dll");
            foreach (string fileName in _files)
            {
                Assembly _assembly = Assembly.LoadFile(fileName);

                if (_assembly != null)
                {
                    Type[] _types = null;
                    try
                    {
                        //获取程序集中定义的类型
                        _types = _assembly.GetTypes();
                    }
                    catch(ReflectionTypeLoadException )// (ReflectionTypeLoadException ex)
                    {

                    }
                    finally
                    {
                        foreach (Type ty in _types)
                        {
                            ///获得一个类型所有实现的接口
                            Type[] _interfaces = ty.GetInterfaces();

                            foreach (Type _ty in _interfaces)
                            {
                                switch (_ty.FullName)
                                {

                                    case "TranMACASims.ICommand":
                                    case "TranMACASims.ITool":
                                    case "TranMACASims.IMenuDef":
                                    case "TranMACASims.IToolBarDef":
                                    case "TranMACASims.IDockableWindowDef":
                                        getPluginObject(_pluginCtner, _ty);
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                    }
                }
            }


            return _pluginCtner;

        }
    }
}
