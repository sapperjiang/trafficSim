using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
//using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
//using ESRI.ArcGIS.Carto;

namespace GISTranSim
{
    interface IApplication
    {
        /// <summary>
        /// Main程序标题
        /// </summary>
        string Caption
        {
            get;
            set;
        }

        /// <summary>
        /// 主程现在使用的工具名称
        /// </summary>
        string CurrentTool
        {
            get;
            set;
        }

        /// <summary>
        /// 主程序储存GIS数据的数据集
        /// </summary>
        DataSet MainDataSet
        {
            get;
            set;
        }

        /// <summary>
        /// 主程序用来存储文档对象
        /// </summary>
        //IMapDocument Document
        //{
        //    get;
        //    set;
        //}

        //ESRI.ArcGIS.Controls.IMapControlDefault MapControl
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 主程序中的pageLayoutControl控件
        ///// </summary>
        //IPageLayoutControlDefault PageLayoutControl
        //{
        //    get;
        //    set;

        //}

        /// <summary>
        /// 主程序的名称
        /// </summary>
        string Name
        {
            get;
            set;
        }

        System.Windows.Forms.Form MainPlatform
        {
            get;
            set;
        }

        StatusBar StatusBar
        {
            get;
            set;
        }

        /// <summary>
        /// 主程序UI界面上的visible
        /// </summary>
        bool Visible
        {
            get;
            set;

        }
    }
}
