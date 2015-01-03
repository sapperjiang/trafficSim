using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
//using ESRI.ArcGIS.Controls;
//using ESRI.ArcGIS.Carto;

namespace GISTranSim
{
    /// <summary>
    /// 表现为一个UI界面上的一个命令按钮，它能够执行一段代码
    /// </summary>
    interface ICommand:IPlugin
    {

        /// <summary>
        /// 命令按钮的图标文件
        /// </summary>
        Bitmap Bitmap
        {
            get;
             
        }

        /// <summary>
        /// 命令按钮的文字
        /// </summary>
        string Caption
        {
            get;
             
        }

        /// <summary>
        /// 命令按钮所属的类别
        /// </summary>
        string Category
        {
            get;
             
        }

        /// <summary>
        /// 命令按钮是否被选中
        /// </summary>
        bool Checked
        {
            get;
             
        }

        /// <summary>
        /// 命令按钮是否可用
        /// </summary>
        bool Enabled
        {
            get;
             
        }

        /// <summary>
        /// 鼠标移到按钮上的状态栏上的提示文字
        /// </summary>
        string Message
        {
            get;
             
        }

        /// <summary>
        /// 按钮名称
        /// </summary>
        string Name
        {
            get;
             
        }

        /// <summary>
        /// 鼠标移动到按钮上弹出的文字
        /// </summary>
        string Tooltip
        {
            get;
             
        }

        /// <summary>
        /// 点击按钮时进行处理的方法
        /// </summary>
        void OnClick();

        /// <summary>
        /// 鼠标移动到按钮上时弹出的文字
        /// </summary>
        void OnCreate(IApplication hook);
    }

}
