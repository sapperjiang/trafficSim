using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GISTranSim
{
    /// <summary>
    /// 在UI界面上表现为一个工具按钮，工具按钮与命令按钮相似，区别是工具按钮点击后开启一个交互过程
    /// 而命令按钮直接执行命令。
    /// </summary>
    interface ITool:IPlugin
    {
        /// <summary>
        /// 工具按钮图标
        /// </summary>
        Bitmap Bitmap
        {
            get;
        }

        /// <summary>
        /// 工具按钮的名称
        /// </summary>
        string Caption
        {
            get;
        }

        /// <summary>
        /// 工具所属的类别
        /// </summary>
        string Category
        {
            get;
        }

        /// <summary>
        /// 工具按钮是否被选择
        /// </summary>
        bool Checked
        {
            get;
        }

        /// <summary>
        /// 工具按钮是否可用
        /// </summary>
        bool Enabled
        {
            get;
        }

        /// <summary>
        /// 鼠标移动到按钮上时状态栏出现的文字
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
        /// tool按钮的提示信息
        /// </summary>
        string Tooltip
        {
            get;
        }

        /// <summary>
        /// tool激活状鼠标状态态的
        /// </summary>
        int Cursor
        {
            get;
        }

        /// <summary>
        /// Tool的激活状态设置
        /// </summary>
        bool Deactivated
        {
            get;
        }

        void OnCreate(IApplication hook);

        void OnDbClick();

        void OnContextMenu(int x, int y);

        /// <summary>
        /// 鼠标在地图上移动的时候触发的事件
        /// </summary>
        void OnMouseMove(int buttion, int shift, int x, int y);

        /// <summary>
        /// 鼠标点击地图时触发的事件
        /// </summary>
        void OnMouseDown(int button, int shift, int x, int y);

        /// <summary>
        /// 鼠标在地图上松开的时候触发的事件
        /// </summary>
        void OnMouseUp();

        /// <summary>
        /// 地图刷新的时候触发
        /// </summary>
        void Refresh(int hDC);

        /// <summary>
        /// 键盘按下时候触发的事件
        /// </summary>
        void OnKeyDown(int keyCode, int shift);

        /// <summary>
        /// 键盘按钮弹起的时候触发的事件
        /// </summary>
        void OnKeyUp(int keyCode, int shift);

        void OnClick();

        /// <summary>
        /// TOOL激活的状态设置
        /// </summary>
        bool Deactivate();
    }
}
