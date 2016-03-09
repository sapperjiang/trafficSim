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
using SubSys_NetworkBuilder;
using SubSys_Graphics;

using System.Runtime.Serialization;

namespace GISTranSim
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
				GraphicsCfger.ScalePixels(2);
			} else {
				GraphicsCfger.ScalePixels(-2);
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
			//		SimController.iCarCount =2;
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
//			SimController.iCarCount =2;
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
			//this.autosiz
			IStaticFactory IFacotry = new StaticFactory();
			
			var roadA= IFacotry.Build(new OxyzPointF(20,20),new OxyzPointF(50,50), EntityType.Road) as Road;
			roadA.Name = "香茗路";
			var roadB= IFacotry.Build(new OxyzPointF(55,55),new OxyzPointF(20,100), EntityType.Road) as Road;
			roadB.Name = "雅馨路";
			
//			var roadC= IFacotry.Build(new OxyzPointF(20,20),new OxyzPointF(50,50), EntityType.Road) as Road;
//			roadC.Name = "居兰路";
//
//			RoadNet roadNetwork = SimController.ISimCtx.RoadNet;
//
//			roadNetwork.AddXNode(rnA);
//			roadNetwork.AddXNode(rnB);
//			roadNetwork.AddXNode(rnC);
			
			
			//roadNetwork.AddXNode(rnD);
			//roadNetwork.AddXNode(rnE);
			//roadNetwork.AddXNode(rnF);
			//roadNetwork.AddXNode(rnG);
			//roadNetwork.AddXNode(rnH);
			//roadNetwork.AddXNode(rnI);

			//SimController.ReA= roadNetwork.AddWay(rnA,rnB);
			//SimController.ReB=roadNetwork.AddWay(rnB,rnC);
			
			
			//创建路由ReA是路由参数
			SimController.ReA1= roadA.Way;
			
			SimController.ReA2 = roadB.Way;

			//            SimController.ReA3 = roadNetwork.AddWay(rnC, rnF);
			//            SimController.ReA4 = roadNetwork.AddWay(rnF, rnH);
			//SimController.ReB1 = roadNetwork.AddWay(rnB, rnE);
			//
			
			//roadNetwork.AddWay(rnB, rnA);
			//

//			foreach (var item in roadNetwork.Ways)
//			{
//				WayFactory.BuildTwoWay(item, 0, 1, 0);
//			}
//			return roadNetwork;
			return null;
			
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
		void ToolStripButtonRectangleClick(object sender, EventArgs e)
		{
			//this.drawArea ;
		}
		#endregion
		
		
		
		//--------drawtools functions--added 2016
		
		
		#region Members

		private DocManager docManager;
		private DragDropManager dragDropManager;
		private MruManager mruManager;
		private PersistWindowState persistState;

		private string argumentFile = "";   // file name from command line

		const string registryPath = "Software\\AlexF\\DrawTools";

		#endregion

		#region Properties

		/// <summary>
		/// File name from the command line
		/// </summary>
		public string ArgumentFile
		{
			get
			{
				return argumentFile;
			}
			set
			{
				argumentFile = value;
			}
		}

		/// <summary>
		/// Get reference to Edit menu item.
		/// Used to show context menu in DrawArea class.
		/// </summary>
		/// <value></value>
		//        public ToolStripMenuItem ContextParent
		//        {
		//            get
		//            {
		//                return editToolStripMenuItem;
		//            }
		//        }

		#endregion

		#region Constructor
//
		//        public MainForm()
		//        {
		//            InitializeComponent();
//
		//            //persistState = new PersistWindowState(registryPath, this);
		//        }

		#endregion

		#region Toolbar Event Handlers

		private void toolStripButtonNew_Click(object sender, EventArgs e)
		{
			CommandNew();
		}

		private void toolStripButtonOpen_Click(object sender, EventArgs e)
		{
			CommandOpen();
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e)
		{
			CommandSave();
		}

		private void toolStripButtonPointer_Click(object sender, EventArgs e)
		{
			CommandPointer();
		}

		private void toolStripButtonRectangle_Click(object sender, EventArgs e)
		{
			CommandRectangle();
		}

		private void toolStripButtonEllipse_Click(object sender, EventArgs e)
		{
			CommandEllipse();
		}

		private void toolStripButtonLine_Click(object sender, EventArgs e)
		{
			CommandLine();
		}

		private void toolStripButtonPencil_Click(object sender, EventArgs e)
		{
			CommandPolygon();
		}

		//        private void toolStripButtonAbout_Click(object sender, EventArgs e)
		//        {
		//            CommandAbout();
		//        }

		private void toolStripButtonUndo_Click(object sender, EventArgs e)
		{
			CommandUndo();
		}

		private void toolStripButtonRedo_Click(object sender, EventArgs e)
		{
			CommandRedo();
		}

		#endregion Toolbar Event Handlers

		#region Menu Event Handlers

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandNew();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandOpen();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandSave();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandSaveAs();
		}

		private void exportToJpgToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandExportToJpg();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			drawArea.GraphicsList.SelectAll();
			drawArea.Refresh();

		}

		private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			drawArea.GraphicsList.UnselectAll();
			drawArea.Refresh();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandDelete command = new CommandDelete(drawArea.GraphicsList);

			if (drawArea.GraphicsList.DeleteSelection())
			{
				drawArea.SetDirty();
				drawArea.Refresh();
				drawArea.AddCommandToHistory(command);
			}
		}

		private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandDeleteAll command = new CommandDeleteAll(drawArea.GraphicsList);

			if (drawArea.GraphicsList.Clear())
			{
				drawArea.SetDirty();
				drawArea.Refresh();
				drawArea.AddCommandToHistory(command);
			}
		}

		private void moveToFrontToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (drawArea.GraphicsList.MoveSelectionToFront())
			{
				drawArea.SetDirty();
				drawArea.Refresh();
			}

		}

		private void moveToBackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (drawArea.GraphicsList.MoveSelectionToBack())
			{
				drawArea.SetDirty();
				drawArea.Refresh();
			}
		}

		private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (drawArea.GraphicsList.ShowPropertiesDialog(drawArea))
			{
				drawArea.SetDirty();
				drawArea.Refresh();
			}

		}

		private void pointerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandPointer();
		}

		private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandRectangle();
		}

		private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandEllipse();
		}

		private void lineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandLine();
		}

		private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandPolygon();
		}

		//        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		//        {
		//            CommandAbout();
		//        }

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandUndo();
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CommandRedo();
		}

		#endregion Menu Event Handlers

		#region DocManager Event Handlers

		/// <summary>
		/// Load document from the stream supplied by DocManager
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void docManager_LoadEvent(object sender, SerializationEventArgs e)
		{
			// DocManager asks to load document from supplied stream
			try
			{
				drawArea.GraphicsList = (GraphicsList)e.Formatter.Deserialize(e.SerializationStream);
			}
			catch (ArgumentNullException ex)
			{
				HandleLoadException(ex, e);
			}
			catch (SerializationException ex)
			{
				HandleLoadException(ex, e);
			}
			catch (SecurityException ex)
			{
				HandleLoadException(ex, e);
			}
		}


		/// <summary>
		/// Save document to stream supplied by DocManager
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void docManager_SaveEvent(object sender, SerializationEventArgs e)
		{
			// DocManager asks to save document to supplied stream
			try
			{
				e.Formatter.Serialize(e.SerializationStream, drawArea.GraphicsList);
			}
			catch (ArgumentNullException ex)
			{
				HandleSaveException(ex, e);
			}
			catch (SerializationException ex)
			{
				HandleSaveException(ex, e);
			}
			catch (SecurityException ex)
			{
				HandleSaveException(ex, e);
			}
		}

		/// <summary>
		/// Export document to jpg stream supplied by DocManager
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void docManager_ExportEvent(object sender, ExportEventArgs e)
		{
			Size size = drawArea.GraphicsList.GetSize();
			int width = size.Width;
			int height = size.Height;

			int stride = GetStride(width, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			Debug.Assert(stride > 0);

			// Define the image palette
			BitmapPalette myPalette = BitmapPalettes.Halftone256;

			byte[] bmpData = new byte[stride * height];
			GCHandle pinnedArray = GCHandle.Alloc(bmpData, GCHandleType.Pinned);
			IntPtr pointer = pinnedArray.AddrOfPinnedObject();
			BitmapSource image = null;

			try
			{

				Image bmp = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppRgb, pointer);

				using (Graphics g = Graphics.FromImage(bmp))
				{
					SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
					g.FillRectangle(brush, new Rectangle(0, 0, width, height));

					if (drawArea.GraphicsList != null)
					{
						drawArea.GraphicsList.Draw(g);
					}
				}

				// Creates a new empty image with the pre-defined palette
				image = BitmapSource.Create(
					width,
					height,
					96,
					96,
					PixelFormats.Bgr32, // Indexed8
					myPalette,
					pointer,
					stride * height,
					stride);
			}
			finally
			{
				//do your stuff
				pinnedArray.Free();
			}

			var stream = e.SaveStream;
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.FlipHorizontal = false;
			encoder.FlipVertical = false;
			encoder.QualityLevel = 30;
			encoder.Rotation = Rotation.Rotate0;
			encoder.Frames.Add(BitmapFrame.Create(image));
			encoder.Save(stream);
		}

		public static int GetStride(int width, System.Drawing.Imaging.PixelFormat format)
		{
			int bitsPerPixel = System.Drawing.Image.GetPixelFormatSize(format);
			Debug.Assert(bitsPerPixel > 0);
			int bytesPerPixel = (bitsPerPixel + 7) / 8;
			int stride = 4 * ((width * bytesPerPixel + 3) / 4);
			return stride;
		}

		#endregion

		#region Event Handlers

		private void MainForm_Load(object sender, EventArgs e)
		{
			// Create draw area
			drawArea.Location = new System.Drawing.Point(0, 0);
			drawArea.Size = new System.Drawing.Size(10, 10);
			drawArea.Owner = null; // this;
			//this.Controls.Add(drawArea);

			// Helper objects (DocManager and others)
			InitializeHelperObjects();

			drawArea.Initialize(this, docManager);
			
			ResizeDrawArea();

			//     LoadSettingsFromRegistry();

			// Submit to Idle event to set controls state at idle time
			Application.Idle += delegate(object o, EventArgs a)
			{
				SetStateOfControls();
			};

			// Open file passed in the command line
			if (ArgumentFile.Length > 0)
				OpenDocument(ArgumentFile);

			// Subscribe to DropDownOpened event for each popup menu
			// (see details in MainForm_DropDownOpened)
			//            foreach (ToolStripItem item in menuStrip.Items)
			//            {
			//                if (item.GetType() == typeof(ToolStripMenuItem))
			//                {
			//                    ((ToolStripMenuItem)item).DropDownOpened += MainForm_DropDownOpened;
			//                }
			//            }
		}

		/// <summary>
		/// Resize draw area when form is resized
		/// </summary>
		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (this.WindowState != FormWindowState.Minimized  &&
			    drawArea != null )
			{
				ResizeDrawArea();
			}
		}

		/// <summary>
		/// Form is closing
		/// </summary>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ( e.CloseReason == CloseReason.UserClosing )
			{
				if (!docManager.CloseDocument())
					e.Cancel = true;
			}

			//       SaveSettingsToRegistry();
		}

		/// <summary>
		/// Popup menu item (File, Edit ...) is opened.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainForm_DropDownOpened(object sender, EventArgs e)
		{
			// Reset active tool to pointer.
			// This prevents bug in rare case when non-pointer tool is active, user opens
			// main main menu and after this clicks in the drawArea. MouseDown event is not
			// raised in this case (why ??), and MouseMove event works incorrectly.
			drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
		}


		#endregion Event Handlers

		#region Other Functions

		/// <summary>
		/// Set state of controls.
		/// Function is called at idle time.
		/// </summary>
		public void SetStateOfControls()
		{
			// Select active tool
			toolStripButtonPointer.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Pointer);
			toolStripButtonRectangle.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Rectangle);
			toolStripButtonEllipse.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Ellipse);
			toolStripButtonLine.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Line);
			toolStripButtonPencil.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Polygon);

			//            pointerToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Pointer);
			//            rectangleToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Rectangle);
			//            ellipseToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Ellipse);
			//            lineToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Line);
			//            pencilToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Polygon);

			bool objects = (drawArea.GraphicsList.Count > 0);
			bool selectedObjects = (drawArea.GraphicsList.SelectionCount > 0);

			// File operations
			//            saveToolStripMenuItem.Enabled = objects;
			toolStripButtonSave.Enabled = objects;
			//            saveAsToolStripMenuItem.Enabled = objects;

			// Edit operations
			//            deleteToolStripMenuItem.Enabled = selectedObjects;
			//            deleteAllToolStripMenuItem.Enabled = objects;
			//            selectAllToolStripMenuItem.Enabled = objects;
			//            unselectAllToolStripMenuItem.Enabled = objects;
			//            moveToFrontToolStripMenuItem.Enabled = selectedObjects;
			//            moveToBackToolStripMenuItem.Enabled = selectedObjects;
			//            propertiesToolStripMenuItem.Enabled = selectedObjects;

			// Undo, Redo
			//undoToolStripMenuItem.Enabled = drawArea.CanUndo;
			toolStripButtonUndo.Enabled = drawArea.CanUndo;

			//  redoToolStripMenuItem.Enabled = drawArea.CanRedo;
			toolStripButtonRedo.Enabled = drawArea.CanRedo;
		}

		/// <summary>
		/// Set draw area to all form client space except toolbar
		/// </summary>
		private void ResizeDrawArea()
		{
			Rectangle rect = this.ClientRectangle;

			drawArea.Left = rect.Left;
			drawArea.Top = rect.Top + this.menuBar.Height + toolStrip.Height;
			drawArea.Width = rect.Width;
			drawArea.Height = rect.Height - menuBar.Height - toolStrip.Height;
		}

		/// <summary>
		/// Initialize helper objects from the DocToolkit Library.
		/// 
		/// Called from Form1_Load. Initialized all objects except
		/// PersistWindowState wich must be initialized in the
		/// form constructor.
		/// </summary>
		private void InitializeHelperObjects()
		{
			// DocManager

			DocManagerData data = new DocManagerData();
			data.FormOwner = this;
			data.UpdateTitle = true;
			data.FileDialogFilter = "DrawTools files (*.dtl)|*.dtl|All Files (*.*)|*.*";
			data.NewDocName = "Untitled.dtl";
			data.RegistryPath = registryPath;

			docManager = new DocManager(data);
			//docManager.RegisterFileType("dtl", "dtlfile", "DrawTools File");

			// Subscribe to DocManager events.
			docManager.SaveEvent += docManager_SaveEvent;
			docManager.LoadEvent += docManager_LoadEvent;
			docManager.ExportEvent += docManager_ExportEvent;

			// Make "inline subscription" using anonymous methods.
			docManager.OpenEvent += delegate(object sender, OpenFileEventArgs e)
			{
				// Update MRU List
				if (e.Succeeded)
					mruManager.Add(e.FileName);
				else
					mruManager.Remove(e.FileName);
			};

			docManager.DocChangedEvent += delegate(object o, EventArgs e)
			{
				drawArea.Refresh();
				drawArea.ClearHistory();
			};

			docManager.ClearEvent += delegate(object o, EventArgs e)
			{
				if (drawArea.GraphicsList != null)
				{
					drawArea.GraphicsList.Clear();
					drawArea.ClearHistory();
					drawArea.Refresh();
				}
			};

			docManager.NewDocument();

			// DragDropManager
			dragDropManager = new DragDropManager(this);
			dragDropManager.FileDroppedEvent += delegate(object sender, FileDroppedEventArgs e)
			{
				OpenDocument(e.FileArray.GetValue(0).ToString());
			};

			// MruManager
			mruManager = new MruManager();
			//            mruManager.Initialize(
			//                this,                              // owner form
			//              //  recentFilesToolStripMenuItem,      // Recent Files menu item
			//     //           fileToolStripMenuItem,            // parent
			//                registryPath);                     // Registry path to keep MRU list

			mruManager.MruOpenEvent += delegate(object sender, MruFileOpenEventArgs e)
			{
				OpenDocument(e.FileName);
			};
		}
		/// <summary>
		/// Handle exception from docManager_LoadEvent function
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fileName"></param>
		private void HandleLoadException(Exception ex, SerializationEventArgs e)
		{
			MessageBox.Show(this,
			                "Open File operation failed. File name: " + e.FileName + "\n" +
			                "Reason: " + ex.Message,
			                Application.ProductName);

			e.Error = true;
		}

		/// <summary>
		/// Handle exception from docManager_SaveEvent function
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fileName"></param>
		private void HandleSaveException(Exception ex, SerializationEventArgs e)
		{
			MessageBox.Show(this,
			                "Save File operation failed. File name: " + e.FileName + "\n" +
			                "Reason: " + ex.Message,
			                Application.ProductName);

			e.Error = true;
		}

		/// <summary>
		/// Open document.
		/// Used to open file passed in command line or dropped into the window
		/// </summary>
		/// <param name="file"></param>
		public void OpenDocument(string file)
		{
			docManager.OpenDocument(file);
		}

		/// <summary>
		/// Load application settings from the Registry
		/// </summary>
		//        void LoadSettingsFromRegistry()
		//        {
		//            try
		//            {
		//                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
//
		//                DrawObject.LastUsedColor = System.Drawing.Color.FromArgb((int)key.GetValue(
		//                    "Color",
		//                    System.Drawing.Color.Black.ToArgb()));
//
		//                DrawObject.LastUsedPenWidth = (int)key.GetValue(
		//                    "Width",
		//                    1);
		//            }
		//            catch (ArgumentNullException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (SecurityException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (ArgumentException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (ObjectDisposedException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (UnauthorizedAccessException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//        }

		/// <summary>
		/// Save application settings to the Registry
		/// </summary>
		//        void SaveSettingsToRegistry()
		//        {
		//            try
		//            {
		//                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
//
		//                key.SetValue("Color", DrawObject.LastUsedColor.ToArgb());
		//                key.SetValue("Width", DrawObject.LastUsedPenWidth);
		//            }
		//            catch (SecurityException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (ArgumentException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (ObjectDisposedException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//            catch (UnauthorizedAccessException ex)
		//            {
		//                HandleRegistryException(ex);
		//            }
		//        }

		private void HandleRegistryException(Exception ex)
		{
			Trace.WriteLine("Registry operation failed: " + ex.Message);
		}

		/// <summary>
		/// Set Pointer draw tool
		/// </summary>
		private void CommandPointer()
		{
			drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
		}

		/// <summary>
		/// Set Rectangle draw tool
		/// </summary>
		private void CommandRectangle()
		{
			drawArea.ActiveTool = DrawArea.DrawToolType.Rectangle;
		}

		/// <summary>
		/// Set Ellipse draw tool
		/// </summary>
		private void CommandEllipse()
		{
			drawArea.ActiveTool = DrawArea.DrawToolType.Ellipse;
		}

		/// <summary>
		/// Set Line draw tool
		/// </summary>
		private void CommandLine()
		{
			drawArea.ActiveTool = DrawArea.DrawToolType.Line;
		}

		/// <summary>
		/// Set Polygon draw tool
		/// </summary>
		private void CommandPolygon()
		{
			drawArea.ActiveTool = DrawArea.DrawToolType.Polygon;
		}

		/// <summary>
		/// Show About dialog
		/// </summary>
		//        private void CommandAbout()
		//        {
		//            FrmAbout frm = new FrmAbout();
		//            frm.ShowDialog(this);
		//        }

		/// <summary>
		/// Open new file
		/// </summary>
		private void CommandNew()
		{
			docManager.NewDocument();
		}

		/// <summary>
		/// Open file
		/// </summary>
		private void CommandOpen()
		{
			docManager.OpenDocument("");
		}

		/// <summary>
		/// Save file
		/// </summary>
		private void CommandSave()
		{
			docManager.SaveDocument(DocManager.SaveType.Save);
		}

		/// <summary>
		/// Save As
		/// </summary>
		private void CommandSaveAs()
		{
			docManager.SaveDocument(DocManager.SaveType.SaveAs);
		}

		/// <summary>
		/// Export current graph document to jpg.
		/// </summary>
		private void CommandExportToJpg()
		{
			docManager.ExportToJpg();
		}

		/// <summary>
		/// Undo
		/// </summary>
		private void CommandUndo()
		{
			drawArea.Undo();
		}

		/// <summary>
		/// Redo
		/// </summary>
		private void CommandRedo()
		{
			drawArea.Redo();
		}

		#endregion

		private void toolStripStatusLabel_Click(object sender, EventArgs e)
		{

		}

		private void drawArea_Load(object sender, EventArgs e)
		{

		}


		void SimMainLoad(object sender, EventArgs e)
		{
			InitializeHelperObjects();

			drawArea.Initialize(this, docManager);
			ResizeDrawArea();

			//removed by sapperjiang
			//    LoadSettingsFromRegistry();

			// Submit to Idle event to set controls state at idle time
			Application.Idle += delegate(object o, EventArgs a)
			{
				SetStateOfControls();
			};

			// Open file passed in the command line
			if (ArgumentFile.Length > 0)
				OpenDocument(ArgumentFile);

			// Subscribe to DropDownOpened event for each popup menu
			// (see details in MainForm_DropDownOpened)
			foreach (ToolStripItem item in this.toolStrip.Items)
			{
				if (item.GetType() == typeof(ToolStripMenuItem))
				{
					((ToolStripMenuItem)item).DropDownOpened += MainForm_DropDownOpened;
				}
			}
		}
		void ToolStripButtonPencilClick(object sender, EventArgs e)
		{
			this.CommandPolygon();
		}
		void ToolStripButtonLineClick(object sender, EventArgs e)
		{
			this.CommandLine();
		}
		void ToolStripButtonUndoClick(object sender, EventArgs e)
		{
			this.CommandUndo();
		}
		void ToolStripButtonRedoClick(object sender, EventArgs e)
		{
			this.CommandRedo();
		}
		void MenuBar_Help_About_Click(object sender, EventArgs e)
		{
			var About = new UIHelpAbout();
			About.ShowDialog();
			
		}
		void ToolStripButtonSaveClick(object sender, EventArgs e)
		{
			this.CommandSave();
		}
		void ToolStripButtonOpenClick(object sender, EventArgs e)
		{
			this.CommandOpen();
		}
		void 重做ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		
		
		
	}
}
