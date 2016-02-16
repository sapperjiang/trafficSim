using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISTranSim
{
    interface IMenuDef:IPlugin
    {
        /// <summary>
        /// 菜单栏标题
        /// </summary>
        string Caption
        {
            get;
             
        }

        /// <summary>
        /// 菜单栏名称
        /// </summary>
        string Name
        {
            get;
             
        }

        /// <summary>
        /// 踩烂来携带的item的数量
        /// </summary>
        int ItemCount
        {
            get;
             
        }

        void getItemInfo(int pos, IItemdef itemdef);
    }
}
