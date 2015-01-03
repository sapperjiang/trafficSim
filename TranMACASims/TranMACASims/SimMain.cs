using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;
using System.Threading;
using SubSys_SimDriving.SysSimContext.Service;
using SubSys_Graphics;
using SubSys_DataVisualization;
using GISTranSim.Input;
using GISTranSim.DataOutput;

namespace GISTranSim
{
	public partial class SimMain : Form
	{
		public SimMain()
		{
			InitializeComponent();
			this.MouseWheel += new MouseEventHandler(SimCartoon_MouseWheel);
			
			//开启路网平移
			this.MouseDown+=PanMap_MouseDown;
			this.MouseUp +=PanMap_MouseUp;
		}

		/// <summary>
		/// 鼠标滚轮放大缩小路网
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SimCartoon_MouseWheel(object sender, MouseEventArgs e)
		{
			this.Invalidate();

			if (e.Delta > 0)
			{
				GUISettings.iGUI_CellPixels += 1;
			}
			else
			{
				GUISettings.iGUI_CellPixels -= 1;
			}
		}
		
		RoadNetWork _roadNetWork = RoadNetWork.GetInstance();
		
		protected override void OnClosing(CancelEventArgs e)
		{
			SimController.bIsExit = true;
			base.OnClosing(e);
		}

		#region 编辑道路节点
		
		bool IsAddNodeActivated = false;
		MouseEventHandler AddNodeHandler;
		private void BT_AddNode_Click(object sender, EventArgs e)
		{
			this.IsAddNodeActivated = !this.IsAddNodeActivated;
			if (this.IsAddNodeActivated == true)
			{
				if (AddNodeHandler == null)
				{
					AddNodeHandler = new MouseEventHandler(AddNode_MouseMove);
				}
				this.MouseMove += AddNodeHandler;
			}
			else
			{
				this.MouseMove -= AddNodeHandler;
				this.TP_MousePos.Active = false;
			}
		}

		Point p = new Point(-1, -1);
		void AddNode_MouseMove(object sender, MouseEventArgs e)
		{
			if (p.X == -1)
			{
				p = e.Location;
			}
			if (p.X !=e.X)
			{
				this.TP_MousePos.Active = true;

				string strTip = string.Format("X:{0} Y:{1}",e.Location.X,e.Location.Y);
				this.TP_MousePos.Show(strTip, this, e.Location);
				//this.TP_MousePos.Active = true;

				p = e.Location;
			}
		}
		#endregion
		
		
		#region 路网平移模式

		Point pStart;
		private void PanMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Middle) {
				this.TSSL_MsgTip.Text = "路网处于平移模式";
				this.Cursor = Cursors.Hand;
				pStart = new Point(e.X, e.Y);
			}
		}
		private void PanMap_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Middle) {

				this.Invalidate();//界面重绘 要不然，两次绘制的界面叠加到了一起了。
				Coordinates.GUI_Offset = new Point(pStart.X - e.X, pStart.Y - e.Y);
				this.TSSL_MsgTip.Text = string.Empty;// "退出了平移模式";
				this.Cursor = Cursors.Arrow;
			}
			
		}

//		
//		MouseEventHandler PanMapMouseDownHandler;
//		MouseEventHandler PanMapMouseUpHandler;

		#endregion
//
//
//		private void SimMain_Load(object sender, EventArgs e)
//		{
////			this.IsPanActivated = !this.IsPanActivated;
////			if (this.IsPanActivated == true)
////			{
//			
////			}
////			else
////			{
////				this.MouseDown -= PanMapMouseDownHandler;
////				this.MouseUp -= PanMapMouseUpHandler;
////			}
//			
//		}
//		
		
		void MemuBar_File_CreateNetWork_Click(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}
		
		#region 仿真环境配置区域
		
		/// <summary>
		/// 配置仿真环境
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MenuBar_File_ConfigEnvr_Click(object sender, System.EventArgs e)
		{
			this.WindowState = FormWindowState.Maximized;
			
			GISTranSim.SimConfig cs = new GISTranSim.SimConfig();
			if (cs.ShowDialog() == DialogResult.OK)
			{
				SimController.iCarCount = cs.iCarCount;
				SimController.iRoadWidth = cs.iRoadLength;
				SimController.iSimInterval = cs.iSimSpeed;
				ModelSetting.dRate = cs.dRatio;
				
			}cs.Dispose();
			
			SimController.ConfigSimEnvironment(this);
			
		}
		#endregion
		
		#region 仿真控制区域
		
		/// <summary>
		/// 启动仿真非单步执行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MenuBar_Simluate_RunConstantly(object sender, System.EventArgs e)
		{
			
			SimController.bIsExit = false;
			
			//this.BT_ConfigEnvr.Enabled = false;
			menuBarConfigSimEnvr.Enabled =false;
			SimController.Start();
			//		BT_SimStart.Enabled = false;
			menuBarSimulateSustained.Enabled =false;
			//throw new NotImplementedException();
			//SimController.
			
		}
		
		
		void MenuBar_Simluate_RunStop_Click(object sender, System.EventArgs e)
		{
			SimController.bIsExit = true;
			//this.BT_SimStart.Enabled = true;
			//	MenubarSim
			menuBarSimulateSustained.Enabled=false;
			//throw new NotImplementedException();
		}
		#endregion
		
		
		#region 数据画图区域

		void MenuBar_Data_DataOutPut_Click(object sender, System.EventArgs e)
		{
			DataOutputer dop = new DataOutputer();
			dop.Show();
			//throw new NotImplementedException();
		}
		/// <summary>
		/// 速度时间图
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MenuBar_Data_SpeedTime_Click(object sender, System.EventArgs e)
		{
			
			AbstractCharterForm abc =new MeanSpeedCharter();
			SpeedTimeCharter st = new SpeedTimeCharter();
			st.Show();
			//throw new NotImplementedException();
		}
		

		void MenuBar_Data_RoadMeanTime_Click(object sender, System.EventArgs e)
		{
			AbstractCharterForm abc =new MeanSpeedCharter();
			abc.Show();
			//throw new NotImplementedException();
		}
		void MenuBar_Data_TimeSpace_Click(object sender, System.EventArgs e)
		{
			AbstractCharterForm abc = new TimeSpaceCharter();
			abc.Show();
			//throw new NotImplementedException();
		}
		void MenuBarSimulateRunStopClick(object sender, System.EventArgs e)
		{
			SimController.bIsExit = true;
			//this.BT_SimStart.Enabled = true;
			//	MenubarSim
			menuBarSimulateSustained.Enabled=false;
			//throw new NotImplementedException();
			//throw new NotImplementedException();
		}
		void MenuBarSimulateRunResumeClick(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}
		void MenubarSimlateStopClick(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}
		#endregion
		
		
	}
}
