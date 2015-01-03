using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace GISTranSim
{
    interface IDockableWndDef
    {
        /// <summary>
        /// 浮动窗体标题
        /// </summary>
        string Caption
        {
            get;
        }

        /// <summary>
        /// 浮动窗体上停靠的子控件
        /// </summary>
        Control ChildWND
        {
            get;
        }

        /// <summary>
        /// 浮动窗体的名称
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 浮动窗体与主框架之间用于交互的额外辅助数据对象
        /// </summary>
        object UserData
        {
            get;
        }

        void OnCreate(IApplication hook);

        void OnDestroy();
    }
}
