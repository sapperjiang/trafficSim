using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SubSys_DataVisualization;
using SubSys_Graphics;
using SubSys_MathUtility;

using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using GISTranSim.Data;



namespace GISTranSim
{
	public partial class SimMain : Form
	{
		public SimMain()
		{
			InitializeComponent();
			
			this.WindowState = FormWindowState.Maximized;
			
			
			var color = Color.LightBlue;
			this.BackColor = color;
			this.menuBar.BackColor = color;
			this.statusBar.BackColor =color;
			
			//SimController.InitializePainters(this);
//
//			if (iroadNet ==null) {
//				iroadNet = SimController.ISimCtx.RoadNet;
//			}
			
			///	SimController.ISimCtx.RoadNet.Updated +=SimController.RepaintNetWork;
			
			SimController.Canvas = this;
			
			SimController.iSimInterval =10;
			//开启鼠标滚轮放大缩小屏幕
			this.MouseWheel += new MouseEventHandler(SimCartoon_MouseWheel);
			this.MouseWheel +=new MouseEventHandler(SimController.RepaintNetWork);
		
			//开启路网平移
			this.MouseDown+=PanScreen_MouseDown;
			this.MouseUp +=PanScreen_MouseUp;
			
			//注册每个仿真结束时候发生的事件
			SimController.OnSimulateStoped +=SimulateStopMessage;
			//注册每个仿真时刻变更发生的事件
			SimController.ISimCtx.OnTimeStepChanged+=this.TimeStepChangeMessage;
			
		}
		private void  SimulateStopMessage(object sender, EventArgs e)
		{
			MessageBox.Show("仿真结束");
		}
		
		private void TimeStepChangeMessage(string strMsg)
		{
			this.tslSimTime.Text = strMsg;
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
				GraphicsCfger.ScalePixels(1);
			} else {
				GraphicsCfger.ScalePixels(-1);
			}
			
			//	SimController.RepaintNetWork();
		}
		
		
		protected override void OnClosing(CancelEventArgs e)
		{
			SimController.bIsExit = true;
			//防止内存泄露
			SimController.OnSimulateStoped-=SimulateStopMessage;
			base.OnClosing(e);
		}
		
		#region 路网平移模式

		Point pStart;
		private void PanScreen_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Middle) {
				this.tsslMsgTip.Text = "路网处于平移模式";
				this.Cursor = Cursors.Hand;
				pStart = new Point(e.X, e.Y);
			}
		}
		private void PanScreen_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Middle) {
				//界面重绘 要不然，两次绘制的界面叠加到了一起了。
				this.Invalidate();
				Coordinates.GraphicsOffset = new Point(pStart.X - e.X, pStart.Y - e.Y);
				this.tsslMsgTip.Text = string.Empty;// "退出了平移模式";
				this.Cursor = Cursors.Arrow;
			}
		}
		
		void MemuBar_File_CreateNetWork_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("尚未实现!");
		}
		#endregion
		
		#region 仿真环境配置区域
		
		/// <summary>
		/// 配置仿真环境
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MenuBar_File_ConfigEnvr_Click(object sender, System.EventArgs e)
		{
			SimController.iCarCount =2;
			SimController.iRoadWidth = 100;
			SimController.iSimInterval = 1000;
			ModelSetting.dRate = 0.85;
			
//			SimController.ConfigSimEnvironment(this);
			
			this.LoadRoadNetwork();
		//	SimController.InitializePainters(this);
			
			//打开仿真运行的按钮
			this.menuBarSimulateSustained.Enabled = true;
//
		}
//
//		protected override void OnPaint(PaintEventArgs e)
//		{
//			base.OnPaint(e);
//
//				SimController.RepaintNetWork();
//		}

		void MenuBar_Config_FormBackColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();//新建颜色对话框
			var result = dialog.ShowDialog();//打开颜色对话框，并接收对话框操作结果
			
			if (result == DialogResult.OK)//如果用户点击OK
			{
				var color= dialog.Color;
				this.BackColor = color;
//				this.menuBar.BackColor =color;
			}
		}
		
		#endregion
		
		#region 仿真控制区域
		
		/// <summary>
		/// 启动仿真非单步执行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MenuBar_SimluateSustained_Click(object sender, System.EventArgs e)
		{
			//----------------------------------------------------
			SimController.iCarCount =2;
			SimController.iRoadWidth = 20;
			SimController.iSimInterval = 100;
			ModelSetting.dRate = 0.85;
			
			//SimController.ConfigSimEnvironment(this);
			
			this.LoadRoadNetwork();
			
			
			//when pause repaint network
			//frMain.MouseWheel+=RepaintNetWork;
			
			//打开仿真运行的按钮
			this.menuBarSimulateSustained.Enabled = true;
			//-------------------------------------------------
			SimController.bIsExit = false;
			menuBarConfigSimEnvr.Enabled =false;
			//SimController.Run();
			SimController.StartSimulate();
			
			menuBarSimulateSustained.Enabled =false;
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
			
			AbstractCharter abc =new MeanSpeedCharter();
			SpeedTimeCharter st = new SpeedTimeCharter();
			st.Show();
			//throw new NotImplementedException();
		}
		
		void MenuBar_Data_RoadMeanTime_Click(object sender, System.EventArgs e)
		{
			AbstractCharter abc =new MeanSpeedCharter();
			abc.Show();
			//throw new NotImplementedException();
		}
		void MenuBar_Data_TimeSpace_Click(object sender, System.EventArgs e)
		{
			AbstractCharter abc = new TimeSpaceCharter();
			abc.Show();
			//throw new NotImplementedException();
		}
		void MenuBar_Simulate_Pause_Click(object sender, System.EventArgs e)
		{
			SimController.bIsPause = true;
			menuBarSimulateSustained.Enabled=false;
		}
		void MenuBar_Simulate_Resume_Click(object sender, System.EventArgs e)
		{
			SimController.bIsPause =false;
//			throw new NotImplementedException();
		}
		void Menubar_Simlate_Stop_Click(object sender, System.EventArgs e)
		{
			SimController.bIsExit = true;
			//throw new NotImplementedException();
//			SimController.sto
		}
		void MenuBar_Config_Parameter_Click(object sender, System.EventArgs e)
		{
			GISTranSim.SimConfig cs = new GISTranSim.SimConfig();
			if (cs.ShowDialog() == DialogResult.OK)
			{
				SimController.iRoadWidth = cs.iRoadLength;
				SimController.iSimInterval = cs.iSimSpeed;
				ModelSetting.dRate = cs.dRatio;
				
			}cs.Dispose();
			
			//throw new NotImplementedException();
		}
		#endregion
		#region 路网加载函数
		private  RoadNet LoadRoadNetwork()
		{
//			this.autosiz
			
			IFactory iabstractFacotry = new StaticFactory();
			
			int iRoadWidth = SimController.iRoadWidth;
			int iBase = 2;
			XNode rnA= iabstractFacotry.Build(new XNodeBuildCmd(new Point(iBase,20)),EntityType.XNode) as XNode;
			XNode rnB = iabstractFacotry.Build(new XNodeBuildCmd(new Point(iBase + iRoadWidth, 20)), EntityType.XNode) as XNode;
			XNode rnC = iabstractFacotry.Build(new XNodeBuildCmd(new Point(iBase + 2 * iRoadWidth, 20)), EntityType.XNode) as XNode;
//			XNode rnD = iabstractFacotry.Build(new XNodeBuildCmd(new Point(iBase, 70)), EntityType.XNode) as XNode;
//			XNode rnE = iabstractFacotry.Build(new XNodeBuildCmd(new Point(iBase + iRoadWidth, 70)), EntityType.XNode) as XNode;
//			XNode rnF = iabstractFacotry.Build(new  XNodeBuildCmd(new Point(iBase + 2 * iRoadWidth, 70)), EntityType.XNode) as XNode;
//			XNode rnG = iabstractFacotry.Build(new XNodeBuildCmd(new Point(iBase, 120)), EntityType.XNode) as XNode;
//			XNode rnH = iabstractFacotry.Build(new  XNodeBuildCmd(new Point(iBase + iRoadWidth, 120)), EntityType.XNode) as XNode;
//			XNode rnI = iabstractFacotry.Build(new  XNodeBuildCmd(new Point(iBase + 2 * iRoadWidth, 120)), EntityType.XNode) as XNode;

			
			RoadNet   roadNetwork = SimController.ISimCtx.RoadNet;
			
			roadNetwork.AddXNode(rnA);
			roadNetwork.AddXNode(rnB);
			roadNetwork.AddXNode(rnC);
//			roadNetwork.AddXNode(rnD);
//			roadNetwork.AddXNode(rnE);
//			roadNetwork.AddXNode(rnF);
//			roadNetwork.AddXNode(rnG);
//			roadNetwork.AddXNode(rnH);
//			roadNetwork.AddXNode(rnI);

			//SimController.ReA= roadNetwork.AddWay(rnA,rnB);
			//	SimController.ReB=roadNetwork.AddWay(rnB,rnC);
			
			
			//创建路由ReA是路由参数
			SimController.ReA1= roadNetwork.AddWay(rnA,rnB);
			SimController.ReA1.Name = "xiangming";
			
			SimController.ReA2 = roadNetwork.AddWay(rnB,rnC);
			
			SimController.ReA2.Name = "lixing";
			
			//            SimController.ReA3 = roadNetwork.AddWay(rnC, rnF);
			//            SimController.ReA4 = roadNetwork.AddWay(rnF, rnH);
			//SimController.ReB1 = roadNetwork.AddWay(rnB, rnE);
			//
			
			roadNetwork.AddWay(rnB, rnA);
			//
			roadNetwork.AddWay(rnC,rnB);

//			roadNetwork.AddWay(rnD,rnE);
//			roadNetwork.AddWay(rnE,rnD);
//
//			roadNetwork.AddWay(rnE,rnF);
//			roadNetwork.AddWay(rnF,rnE);
//
//			roadNetwork.AddWay(rnG,rnH);
//			roadNetwork.AddWay(rnH,rnG);
//			roadNetwork.AddWay(rnH,rnI);
//			roadNetwork.AddWay(rnI,rnH);
//
//			roadNetwork.AddWay(rnA,rnD);
//			roadNetwork.AddWay(rnD,rnA);
//
//			roadNetwork.AddWay(rnB,rnE);
//			roadNetwork.AddWay(rnE,rnB);
//
//			//roadNetwork.AddWay(rnC,rnF);
//			roadNetwork.AddWay(rnF,rnC);
//
//			roadNetwork.AddWay(rnD,rnG);
//			roadNetwork.AddWay(rnG,rnD);
//
//			roadNetwork.AddWay(rnE,rnH);
//			roadNetwork.AddWay(rnH,rnE);
//
//			roadNetwork.AddWay(rnF,rnI);
//			roadNetwork.AddWay(rnI,rnF);

			foreach (var item in roadNetwork.Ways)
			{
				WayFactory.BuildTwoWay(item, 0, 1, 0);
			}
			return roadNetwork;
			
		}
		
		#endregion
		
		#region 编辑道路节点
		bool _bIsRoadNetEditing = false;
		MouseEventHandler RoadNetEditHandler;

		
		void MenuBar_Edit_RoadNetwork_Click(object sender, System.EventArgs e)
		{
			this._bIsRoadNetEditing = !this._bIsRoadNetEditing;
			if (this._bIsRoadNetEditing == true)
			{
				if (RoadNetEditHandler == null)
				{
					RoadNetEditHandler = new MouseEventHandler(RoadNetEdit_MouseMove);
				}
				this.MouseMove += RoadNetEditHandler;
			}
			else
			{
				this.MouseMove -= RoadNetEditHandler;
				this.tpMousePositonTip.Active = false;
			}
		}
		
		Point p = new Point(-1, -1);
		void RoadNetEdit_MouseMove(object sender, MouseEventArgs e)
		{
			if (p.X == -1)
			{
				p = e.Location;
			}
			if (p.X !=e.X)
			{
				this.tpMousePositonTip.Active = true;

				string strTip = string.Format("X:{0} Y:{1}",e.Location.X,e.Location.Y);
				this.tpMousePositonTip.Show(strTip, this, e.Location);
				p = e.Location;
			}
		}
		#endregion
	}
}
