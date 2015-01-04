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
using SubSys_SimDriving.ModelFactory;


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
			
			//开启鼠标滚轮放大缩小屏幕
			this.MouseWheel += new MouseEventHandler(SimCartoon_MouseWheel);
			
			//开启路网平移
			this.MouseDown+=PanMap_MouseDown;
			this.MouseUp +=PanMap_MouseUp;
			
			SimController.OnSimulateOver +=SimOverMessageShow;
			
		}
		private void  SimOverMessageShow(object sender, EventArgs e)
		{
			MessageBox.Show("仿真结束");
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
				GraphicsConfiger.ScaleCellPixels(1);
			} else {
				GraphicsConfiger.ScaleCellPixels(-1);
			}
			
		}
		
		RoadNetWork _roadNetWork = RoadNetWork.GetInstance();
		
		protected override void OnClosing(CancelEventArgs e)
		{
			SimController.bIsExit = true;
			//防止内存泄露
			SimController.OnSimulateOver-=SimOverMessageShow;
			
			base.OnClosing(e);
		}

		#region 编辑道路节点
		
//		bool IsAddNodeActivated = false;
//		//MouseEventHandler AddNodeHandler;
//		private void BT_AddNode_Click(object sender, EventArgs e)
//		{
//			this.IsAddNodeActivated = !this.IsAddNodeActivated;
//			if (this.IsAddNodeActivated == true)
//			{
//				if (AddNodeHandler == null)
//				{
//					AddNodeHandler = new MouseEventHandler(AddNode_MouseMove);
//				}
//				this.MouseMove += AddNodeHandler;
//			}
//			else
//			{
//				this.MouseMove -= AddNodeHandler;
//				this.TP_MousePos.Active = false;
//			}
//		}
//
//		Point p = new Point(-1, -1);
//		void AddNode_MouseMove(object sender, MouseEventArgs e)
//		{
//			if (p.X == -1)
//			{
//				p = e.Location;
//			}
//			if (p.X !=e.X)
//			{
//				this.TP_MousePos.Active = true;
//
//				string strTip = string.Format("X:{0} Y:{1}",e.Location.X,e.Location.Y);
//				this.TP_MousePos.Show(strTip, this, e.Location);
//				//this.TP_MousePos.Active = true;
//
//				p = e.Location;
//			}
//		}
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
				Coordinates.GraphicsOffset = new Point(pStart.X - e.X, pStart.Y - e.Y);
				this.TSSL_MsgTip.Text = string.Empty;// "退出了平移模式";
				this.Cursor = Cursors.Arrow;
			}
			
		}

		
		void MemuBar_File_CreateNetWork_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("尚未实现!");
			//	throw new NotImplementedException();
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
			
			SimController.iCarCount =40;//cs.iCarCount;
			SimController.iRoadWidth = 100;//cs.iRoadLength;
			SimController.iSimInterval = 100;//cs.iSimSpeed;
			ModelSetting.dRate = 0.85;//cs.dRatio;
			
			SimController.ConfigSimEnvironment(this);
			this.menuBarSimulateSustained.Enabled = true;
//			this.LoadRoadNetwork();
//			SimController.InitializePaintService(this);
//
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
			
			SimController.bIsExit = false;
			
			//this.BT_ConfigEnvr.Enabled = false;
			menuBarConfigSimEnvr.Enabled =false;
			SimController.Start();
			//		BT_SimStart.Enabled = false;
			menuBarSimulateSustained.Enabled =false;
			//throw new NotImplementedException();
			//SimController.
			
		}
		
		
		void MenuBar_SimluateStop_Click(object sender, System.EventArgs e)
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
		void MenuBar_Simulate_Pause_Click(object sender, System.EventArgs e)
		{
			SimController.bIsPause = true;
			//this.BT_SimStart.Enabled = true;
			//	MenubarSim
			menuBarSimulateSustained.Enabled=false;
			//throw new NotImplementedException();
			//throw new NotImplementedException();
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
				SimController.iCarCount = cs.iCarCount;
				SimController.iRoadWidth = cs.iRoadLength;
				SimController.iSimInterval = cs.iSimSpeed;
				ModelSetting.dRate = cs.dRatio;
				
			}cs.Dispose();
			
			//throw new NotImplementedException();
		}
		#endregion
		
		
		
		#region 路网加载函数
		private  RoadNetWork LoadRoadNetwork()
		{
			
			IAbstractFactory iabstractFacotry = new TrafficEntityFactory();
			
			int iRoadWidth = SimController.iRoadWidth;
			int iBase = 2;
			RoadNode rnA= iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase,20)),EntityType.RoadNode) as RoadNode;
			RoadNode rnB = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 20)), EntityType.RoadNode) as RoadNode;
			RoadNode rnC = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 20)), EntityType.RoadNode) as RoadNode;
			RoadNode rnD = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase, 70)), EntityType.RoadNode) as RoadNode;
			RoadNode rnE = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 70)), EntityType.RoadNode) as RoadNode;
			RoadNode rnF = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 70)), EntityType.RoadNode) as RoadNode;
			RoadNode rnG = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase, 120)), EntityType.RoadNode) as RoadNode;
			RoadNode rnH = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 120)), EntityType.RoadNode) as RoadNode;
			RoadNode rnI = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 120)), EntityType.RoadNode) as RoadNode;

			
			RoadNetWork   roadNetwork = SimController.ISCtx.NetWork;
			
			roadNetwork.AddRoadNode(rnA);
			roadNetwork.AddRoadNode(rnB);
			roadNetwork.AddRoadNode(rnC);
			roadNetwork.AddRoadNode(rnD);
			roadNetwork.AddRoadNode(rnE);
			roadNetwork.AddRoadNode(rnF);
			roadNetwork.AddRoadNode(rnG);
			roadNetwork.AddRoadNode(rnH);
			roadNetwork.AddRoadNode(rnI);

			SimController.ReA= roadNetwork.AddRoadEdge(rnA,rnB);
			SimController.ReB=roadNetwork.AddRoadEdge(rnB,rnC);
			roadNetwork.AddRoadEdge(rnB, rnA);
			//
			roadNetwork.AddRoadEdge(rnC,rnB);

			roadNetwork.AddRoadEdge(rnD,rnE);
			roadNetwork.AddRoadEdge(rnE,rnD);
			
			roadNetwork.AddRoadEdge(rnE,rnF);
			roadNetwork.AddRoadEdge(rnF,rnE);
			
			roadNetwork.AddRoadEdge(rnG,rnH);
			roadNetwork.AddRoadEdge(rnH,rnG);
			roadNetwork.AddRoadEdge(rnH,rnI);
			roadNetwork.AddRoadEdge(rnI,rnH);
			
			roadNetwork.AddRoadEdge(rnA,rnD);
			roadNetwork.AddRoadEdge(rnD,rnA);
			
			roadNetwork.AddRoadEdge(rnB,rnE);
			roadNetwork.AddRoadEdge(rnE,rnB);
			
			roadNetwork.AddRoadEdge(rnC,rnF);
			roadNetwork.AddRoadEdge(rnF,rnC);
			
			roadNetwork.AddRoadEdge(rnD,rnG);
			roadNetwork.AddRoadEdge(rnG,rnD);
			
			roadNetwork.AddRoadEdge(rnE,rnH);
			roadNetwork.AddRoadEdge(rnH,rnE);
			
			roadNetwork.AddRoadEdge(rnF,rnI);
			roadNetwork.AddRoadEdge(rnI,rnF);

			foreach (var item in roadNetwork.RoadEdges)
			{
				RoadEdgeFacory.BuildTwoWay(item, 1, 1, 1);
			}
			return roadNetwork;
			
		}
		void MenuBar_Config_FormBackColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();//新建颜色对话框
			var result = dialog.ShowDialog();//打开颜色对话框，并接收对话框操作结果
			
			if (result == DialogResult.OK)//如果用户点击OK
			{
				var color= dialog.Color;
				this.BackColor = color;
				this.menuBar.BackColor =color;
//				color = dialog.Color;//获取用户选择的颜色，然后你就可以用这个颜色了
			}
			
			//throw new NotImplementedException();
		}
		#endregion
	}
}
