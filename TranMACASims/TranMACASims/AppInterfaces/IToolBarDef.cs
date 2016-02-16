using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISTranSim
{
    interface IToolBarDef:IPlugin
    {
        /// <summary>
        /// 工具条标题
        /// </summary>
        string Caption
        {
            get;
             
        }

        /// <summary>
        /// 工具条的名称
        /// </summary>
        string Name
        {
            get;
             
        }

        string ItemCount
        {
            get;
             
        }

        void getItemInfo(int pos, IItemdef itemDef);
    }
}
