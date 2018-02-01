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
using SubSys_NetWorkBuilder;

namespace TrafficSim
{
	//to use from mainfrom
	public partial class SimMain :Form
	{
		public SimMain()
		{
			InitializeComponent();
			
			//this.l
			this.WindowState = FormWindowState.Maximized;
			var color = System.Drawing.Color.LightBlue;
			this.BackColor = color;
			this.menuBar.BackColor = color;
			this.statusBar.BackColor =color;

			////开启鼠标滚轮放大缩小屏幕
			//this.MouseWheel += new MouseEventHandler(SimCartoon_MouseWheel);
			//this.MouseWheel +=new MouseEventHandler(SimController.RepaintNetWork);
			
			////开启路网平移
			//this.MouseDown+=PanScreen_MouseDown;
			//this.MouseUp +=PanScreen_MouseUp;

            Canvas simCanvas = new Canvas();
            SplitMain.Panel1.Controls.Add(simCanvas);
            simCanvas.Visible = false;
            SimController.Canvas = simCanvas;

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
				GraphicsSetter.ScaleByPixels(2);
			} else {
				GraphicsSetter.ScaleByPixels(-2);
			}
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
                // 向量的坐标等于终点坐标
                var offsetVector = new OxyzPointF(e.X - pStart.X,  e.Y-pStart.Y );
                Coordinates.GraphicsOffset = offsetVector;

				this.tsslMsgTip.Text = string.Empty;// "退出了平移模式";
				this.Cursor = Cursors.Arrow;
			}
		}
		
		void MemuBar_File_CreateNetWork_Click(object sender, System.EventArgs e)
		{
            //this.CommandNew();
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
			//SimController.iCarCount =2;
			//SimController.iRoadWidth = 100;
			SimController.iSimInterval = 1000;
			ModelSetting.dRate = 0.85;

			//打开仿真运行的按钮
			this.menuBarSimulateStart.Enabled = true;
//
		}


		void MenuBar_Config_FormBackColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();//新建颜色对话框
			var result = dialog.ShowDialog();//打开颜色对话框，并接收对话框操作结果
			
			if (result == DialogResult.OK)//如果用户点击OK
			{
				var color= dialog.Color;
				this.BackColor = color;
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
            //关闭路网编辑器，显示仿真窗口
            netbuilder.Visible = false;

            SimController.Canvas.Visible = true;
            SplitMain.Panel2Collapsed = false;
            //----------------------------------------------------
            //SimController.iRoadWidth = 20;
			SimController.iSimInterval = 100;
			ModelSetting.dRate = 0.85;

            
            SimController.iSimInterval = 10;

            //打开仿真运行的按钮
            this.menuBarSimulateStart.Enabled = true;
			//-------------------------------------------------
			SimController.bIsExit = false;
			menuBarConfigSimEnvr.Enabled =false;
            SimController.StartSimulate();
			
			menuBarSimulateStart.Enabled =false;
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
			menuBarSimulateStart.Enabled=false;
		}
		void MenuBar_Simulate_Resume_Click(object sender, System.EventArgs e)
		{
			SimController.bIsPause =false;
		}
		void Menubar_Simlate_Stop_Click(object sender, System.EventArgs e)
		{
			SimController.bIsExit = true;
            netbuilder.Visible = true;
            SimController.Canvas.Visible = false;
            SplitMain.Panel2Collapsed = true;
		}
		void MenuBar_Config_Parameter_Click(object sender, System.EventArgs e)
		{
			TrafficSim.SimConfig cs = new TrafficSim.SimConfig();
			if (cs.ShowDialog() == DialogResult.OK)
			{
                SimSettings.iCellWidth = cs.iCellWidth;
				SimController.iMobileCount = cs.iCarCount;
				SimController.iSimInterval = cs.iSimSpeed;
				ModelSetting.dRate = cs.dRatio;
				
			}cs.Dispose();
		}
		#endregion
//		#region 路网加载函数


//		private  RoadNet LoadRoadNetwork()
//		{
//			//this.autosiz
//			IStaticFactory IFacotry = new StaticFactory();
			
//			var roadA= IFacotry.Build(new OxyzPointF(20,20),new OxyzPointF(50,50), EntityType.Road) as Road;
//			roadA.Name = "香茗路";
//			var roadB= IFacotry.Build(new OxyzPointF(55,55),new OxyzPointF(20,100), EntityType.Road) as Road;
//			roadB.Name = "雅馨路";
			
////			var roadC= IFacotry.Build(new OxyzPointF(20,20),new OxyzPointF(50,50), EntityType.Road) as Road;
////			roadC.Name = "居兰路";
////
////			RoadNet roadNetwork = SimController.ISimCtx.RoadNet;
////
////			roadNetwork.AddXNode(rnA);
////			roadNetwork.AddXNode(rnB);
////			roadNetwork.AddXNode(rnC);
			
			
//			//roadNetwork.AddXNode(rnD);
//			//roadNetwork.AddXNode(rnE);
//			//roadNetwork.AddXNode(rnF);
//			//roadNetwork.AddXNode(rnG);
//			//roadNetwork.AddXNode(rnH);
//			//roadNetwork.AddXNode(rnI);

//			//SimController.ReA= roadNetwork.AddWay(rnA,rnB);
//			//SimController.ReB=roadNetwork.AddWay(rnB,rnC);
			
			
//			//创建路由ReA是路由参数
//			//SimController.ReA1= roadA.Way;
			
//			//SimController.ReA2 = roadB.Way;

//			//            SimController.ReA3 = roadNetwork.AddWay(rnC, rnF);
//			//            SimController.ReA4 = roadNetwork.AddWay(rnF, rnH);
//			//SimController.ReB1 = roadNetwork.AddWay(rnB, rnE);
//			//
			
//			//roadNetwork.AddWay(rnB, rnA);
//			//

////			foreach (var item in roadNetwork.Ways)
////			{
////				WayFactory.BuildTwoWay(item, 0, 1, 0);
////			}
////			return roadNetwork;
//			return null;
			
//		}
//		
		
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

  //      private void HandleRegistryException(Exception ex)
		//{
		//	Trace.WriteLine("Registry operation failed: " + ex.Message);
		//}

   
        void MenuBar_MenuItem_Undo_Click(object sender, EventArgs e)
        {
            //this.CommandUndo();
        }

        private void menuBarFileSaveNetWork_Click(object sender, EventArgs e)
        {
            //this.CommandSave();
        }

        NetBuilder netbuilder;
        void SimMain_Load(object sender, EventArgs e)
		{
            netbuilder = new NetBuilder();
            SplitMain.Panel1.Controls.Add(netbuilder);
            //netbuilder.FormBorderStyle = FormBorderStyle.
            netbuilder.Visible = true;
            //netbuilder.Dock = DockStyle.Fill;
            //netbuilder.AutoScaleMode= AutoScaleMode.f
            ////SplitterPanel
            //InitializeHelperObjects();

            //drawArea.Initialize(this, docManager);
            //ResizeDrawArea();

            //removed by sapperjiang
            //    LoadSettingsFromRegistry();

            //         // Submit to Idle event to set controls state at idle time
            //         Application.Idle += delegate (object o, EventArgs a)
            //         {
            //             SetStateOfControls();
            //         };

            //// Open file passed in the command line
            //if (ArgumentFile.Length > 0)
            //	OpenDocument(ArgumentFile);

            //// Subscribe to DropDownOpened event for each popup menu
            //// (see details in MainForm_DropDownOpened)
            //foreach (ToolStripItem item in this.toolStrip.Items)
            //{
            //	if (item.GetType() == typeof(ToolStripMenuItem))
            //	{
            //		((ToolStripMenuItem)item).DropDownOpened += MainForm_DropDownOpened;
            //	}
            //}
        }

        void MenuBar_Help_About_Click(object sender, EventArgs e)
		{
			var About = new UIHelpAbout();
			About.ShowDialog();
		}

        private void ShowDebugMessage()
        {
            DebugShower ds = new DebugShower();
            IRoadNet inet = RoadNet.GetInstance();

            List<string> list = new List<string>();
            foreach (var item in inet.Ways)
            {
                list.Add(item.Name);
            }
            ds.LB_LaneNuber.DataSource = list;

            List<string> listLane = new List<string>();

            foreach (var item in inet.Lanes)
            {
                listLane.Add(item.Name+ item.Shape.Count.ToString());
            }

            ds.LB_LaneShape.DataSource = listLane;//.lane.Lanes;// as Dictionary<int, lane>;
            SplitMain.Panel2.Controls.Add(ds);
            ds.Show();
        }

    }
}
