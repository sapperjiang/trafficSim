using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISTranSim
{
    interface IItemdef
    {
        /// <summary>
        /// 该item是否属于一个新组
        /// </summary>
        bool Group
        {
            get;
            set;

        }

        /// <summary>
        /// 该item 的ID
        /// </summary>
        string ID
        {
            get;
            set;
        }

        /// <summary>
        /// item的子类command或Tool
        /// </summary>
        long Subtype
        {
            get;
            set;

        }
    }
}
