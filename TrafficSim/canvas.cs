using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Security;
using System.Runtime.InteropServices;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;
using SubSys_NetBuilder;
using SubSys_Graphics;

using System.Runtime.Serialization;
using System.Collections.Generic;

namespace TrafficSim
{
	//to use from mainfrom
	public partial class Canvas :Form
	{
		public Canvas()
		{
			InitializeComponent();
			
			//this.l
			this.WindowState = FormWindowState.Maximized;
			var color = System.Drawing.Color.LightBlue;
			this.BackColor = color;
            this.TopLevel = false;


			//开启鼠标滚轮放大缩小屏幕
			this.MouseWheel += new MouseEventHandler(SimCartoon_MouseWheel);
			this.MouseWheel +=new MouseEventHandler(SimController.RepaintNetWork);
			
			//开启路网平移
			this.MouseDown+=PanScreen_MouseDown;
			this.MouseUp +=PanScreen_MouseUp;
			
		}
	
		/// <summary>
		/// 鼠标滚轮放大缩小路网
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SimCartoon_MouseWheel(object sender, MouseEventArgs e)
		{
			this.Invalidate();

			if (e.Delta > 0) {
				GraphicsSetter.ScaleByPixels(2);
			} else {
				GraphicsSetter.ScaleByPixels(-2);
			}
		}
	

		Point pStart;
		private void PanScreen_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Middle) {
				//this.tsslMsgTip.Text = "路网处于平移模式";
				this.Cursor = Cursors.Hand;
				pStart = new Point(e.X, e.Y);
			}
		}
		private void PanScreen_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Middle) {
				//界面重绘 要不然，两次绘制的界面叠加到了一起了。
				this.Invalidate();
                // 向量的坐标等于终点坐标
                var offsetVector = new OxyzPointF(e.X - pStart.X,  e.Y-pStart.Y );
                Coordinates.GraphicsOffset = offsetVector;

				//this.tsslMsgTip.Text = string.Empty;// "退出了平移模式";
				this.Cursor = Cursors.Arrow;
			}
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.ClientSize = new System.Drawing.Size(1269, 694);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Canvas";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}
