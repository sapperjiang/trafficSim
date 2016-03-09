using  SubSys_NetworkBuilder;
using System.Windows.Forms;

namespace GISTranSim
{
	partial class SimMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimMain));
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.tslSimTime = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.tsslMsgTip = new System.Windows.Forms.ToolStripStatusLabel();
			this.tpMousePositonTip = new System.Windows.Forms.ToolTip(this.components);
			this.menuBar = new System.Windows.Forms.MenuStrip();
			this.menuBarFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarFileCreateNetwork = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarFileSaveNetWork = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarPrgExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarEditRoadNetwork = new System.Windows.Forms.ToolStripMenuItem();
			this.重做ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.重做RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarConfigSimEnvr = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuBarConfigParameterSet = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuBarConfigFormBackColor = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarSimulate = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarSimulateSustained = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarSimulateRunSingleStep = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarSimulatePause = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarSimulateResume = new System.Windows.Forms.ToolStripMenuItem();
			this.MenubarSimlateStop = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarData = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarDataTimeSpace = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarDataSpeedTime = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarDataRoadMeanSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBarDataOutput = new System.Windows.Forms.ToolStripMenuItem();
			this.图表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.关于ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.关于ToolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.关于ToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonPointer = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPencil = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonLine = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRectangle = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonEllipse = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.drawArea = new SubSys_NetworkBuilder.DrawArea();
			this.撤销UToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
			this.statusBar.SuspendLayout();
			this.menuBar.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBar
			// 
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripStatusLabel1,
			this.tslSimTime,
			this.toolStripSplitButton1,
			this.tsslMsgTip});
			this.statusBar.Location = new System.Drawing.Point(0, 513);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(964, 22);
			this.statusBar.TabIndex = 3;
			this.statusBar.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 17);
			this.toolStripStatusLabel1.Text = "当前时间：";
			// 
			// tslSimTime
			// 
			this.tslSimTime.Name = "tslSimTime";
			this.tslSimTime.Size = new System.Drawing.Size(15, 17);
			this.tslSimTime.Text = "0";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 20);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			// 
			// tsslMsgTip
			// 
			this.tsslMsgTip.Name = "tsslMsgTip";
			this.tsslMsgTip.Size = new System.Drawing.Size(0, 17);
			// 
			// tpMousePositonTip
			// 
			this.tpMousePositonTip.AutoPopDelay = 5000;
			this.tpMousePositonTip.InitialDelay = 500;
			this.tpMousePositonTip.IsBalloon = true;
			this.tpMousePositonTip.ReshowDelay = 100;
			// 
			// menuBar
			// 
			this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarFile,
			this.menuBarEdit,
			this.menuBarConfig,
			this.menuBarSimulate,
			this.menuBarData,
			this.图表ToolStripMenuItem,
			this.关于ToolStripMenuItem});
			this.menuBar.Location = new System.Drawing.Point(0, 0);
			this.menuBar.Name = "menuBar";
			this.menuBar.Size = new System.Drawing.Size(964, 25);
			this.menuBar.TabIndex = 13;
			this.menuBar.Text = "menuStrip1";
			// 
			// menuBarFile
			// 
			this.menuBarFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarFileCreateNetwork,
			this.menuBarFileSaveNetWork,
			this.menuBarPrgExit});
			this.menuBarFile.Name = "menuBarFile";
			this.menuBarFile.Size = new System.Drawing.Size(44, 21);
			this.menuBarFile.Text = "文件";
			// 
			// menuBarFileCreateNetwork
			// 
			this.menuBarFileCreateNetwork.Name = "menuBarFileCreateNetwork";
			this.menuBarFileCreateNetwork.Size = new System.Drawing.Size(124, 22);
			this.menuBarFileCreateNetwork.Text = "新建路网";
			this.menuBarFileCreateNetwork.Click += new System.EventHandler(this.MemuBar_File_CreateNetWork_Click);
			// 
			// menuBarFileSaveNetWork
			// 
			this.menuBarFileSaveNetWork.Enabled = false;
			this.menuBarFileSaveNetWork.Name = "menuBarFileSaveNetWork";
			this.menuBarFileSaveNetWork.Size = new System.Drawing.Size(124, 22);
			this.menuBarFileSaveNetWork.Text = "保存路网";
			// 
			// menuBarPrgExit
			// 
			this.menuBarPrgExit.Name = "menuBarPrgExit";
			this.menuBarPrgExit.Size = new System.Drawing.Size(124, 22);
			this.menuBarPrgExit.Text = "退出";
			// 
			// menuBarEdit
			// 
			this.menuBarEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.重做ToolStripMenuItem,
			this.重做RToolStripMenuItem,
			this.撤销UToolStripMenuItem,
			this.menuBarEditRoadNetwork});
			this.menuBarEdit.Name = "menuBarEdit";
			this.menuBarEdit.Size = new System.Drawing.Size(44, 21);
			this.menuBarEdit.Text = "编辑";
			// 
			// menuBarEditRoadNetwork
			// 
			this.menuBarEditRoadNetwork.Enabled = false;
			this.menuBarEditRoadNetwork.Name = "menuBarEditRoadNetwork";
			this.menuBarEditRoadNetwork.Size = new System.Drawing.Size(152, 22);
			this.menuBarEditRoadNetwork.Text = "编辑路网";
			this.menuBarEditRoadNetwork.Click += new System.EventHandler(this.MenuBar_Edit_RoadNetwork_Click);
			// 
			// 重做ToolStripMenuItem
			// 
			this.重做ToolStripMenuItem.Name = "重做ToolStripMenuItem";
			this.重做ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.重做ToolStripMenuItem.Text = "撤销（U）";
			this.重做ToolStripMenuItem.Click += new System.EventHandler(this.重做ToolStripMenuItemClick);
			// 
			// 重做RToolStripMenuItem
			// 
			this.重做RToolStripMenuItem.Name = "重做RToolStripMenuItem";
			this.重做RToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.重做RToolStripMenuItem.Text = "重做（R)";
			// 
			// menuBarConfig
			// 
			this.menuBarConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarConfigSimEnvr,
			this.MenuBarConfigParameterSet,
			this.MenuBarConfigFormBackColor});
			this.menuBarConfig.Name = "menuBarConfig";
			this.menuBarConfig.Size = new System.Drawing.Size(44, 21);
			this.menuBarConfig.Text = "配置";
			// 
			// menuBarConfigSimEnvr
			// 
			this.menuBarConfigSimEnvr.Enabled = false;
			this.menuBarConfigSimEnvr.Name = "menuBarConfigSimEnvr";
			this.menuBarConfigSimEnvr.Size = new System.Drawing.Size(124, 22);
			this.menuBarConfigSimEnvr.Text = "加载路网";
			this.menuBarConfigSimEnvr.Click += new System.EventHandler(this.MenuBar_File_ConfigEnvr_Click);
			// 
			// MenuBarConfigParameterSet
			// 
			this.MenuBarConfigParameterSet.Enabled = false;
			this.MenuBarConfigParameterSet.Name = "MenuBarConfigParameterSet";
			this.MenuBarConfigParameterSet.Size = new System.Drawing.Size(124, 22);
			this.MenuBarConfigParameterSet.Text = "参数设置";
			this.MenuBarConfigParameterSet.Click += new System.EventHandler(this.MenuBar_Config_Parameter_Click);
			// 
			// MenuBarConfigFormBackColor
			// 
			this.MenuBarConfigFormBackColor.Name = "MenuBarConfigFormBackColor";
			this.MenuBarConfigFormBackColor.Size = new System.Drawing.Size(124, 22);
			this.MenuBarConfigFormBackColor.Text = "背景颜色";
			this.MenuBarConfigFormBackColor.Click += new System.EventHandler(this.MenuBar_Config_FormBackColor_Click);
			// 
			// menuBarSimulate
			// 
			this.menuBarSimulate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarSimulateSustained,
			this.menuBarSimulateRunSingleStep,
			this.menuBarSimulatePause,
			this.menuBarSimulateResume,
			this.MenubarSimlateStop});
			this.menuBarSimulate.Name = "menuBarSimulate";
			this.menuBarSimulate.Size = new System.Drawing.Size(44, 21);
			this.menuBarSimulate.Text = "运行";
			// 
			// menuBarSimulateSustained
			// 
			this.menuBarSimulateSustained.Name = "menuBarSimulateSustained";
			this.menuBarSimulateSustained.Size = new System.Drawing.Size(124, 22);
			this.menuBarSimulateSustained.Text = "启动仿真";
			this.menuBarSimulateSustained.Click += new System.EventHandler(this.MenuBar_SimluateSustained_Click);
			// 
			// menuBarSimulateRunSingleStep
			// 
			this.menuBarSimulateRunSingleStep.Enabled = false;
			this.menuBarSimulateRunSingleStep.Name = "menuBarSimulateRunSingleStep";
			this.menuBarSimulateRunSingleStep.Size = new System.Drawing.Size(124, 22);
			this.menuBarSimulateRunSingleStep.Text = "单步运行";
			// 
			// menuBarSimulatePause
			// 
			this.menuBarSimulatePause.Name = "menuBarSimulatePause";
			this.menuBarSimulatePause.Size = new System.Drawing.Size(124, 22);
			this.menuBarSimulatePause.Text = "暂停仿真";
			this.menuBarSimulatePause.Click += new System.EventHandler(this.MenuBar_Simulate_Pause_Click);
			// 
			// menuBarSimulateResume
			// 
			this.menuBarSimulateResume.Name = "menuBarSimulateResume";
			this.menuBarSimulateResume.Size = new System.Drawing.Size(124, 22);
			this.menuBarSimulateResume.Text = "恢复仿真";
			this.menuBarSimulateResume.Click += new System.EventHandler(this.MenuBar_Simulate_Resume_Click);
			// 
			// MenubarSimlateStop
			// 
			this.MenubarSimlateStop.Name = "MenubarSimlateStop";
			this.MenubarSimlateStop.Size = new System.Drawing.Size(124, 22);
			this.MenubarSimlateStop.Text = "结束仿真";
			this.MenubarSimlateStop.Click += new System.EventHandler(this.Menubar_Simlate_Stop_Click);
			// 
			// menuBarData
			// 
			this.menuBarData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarDataTimeSpace,
			this.menuBarDataSpeedTime,
			this.menuBarDataRoadMeanSpeed,
			this.menuBarDataOutput});
			this.menuBarData.Name = "menuBarData";
			this.menuBarData.Size = new System.Drawing.Size(44, 21);
			this.menuBarData.Text = "数据";
			// 
			// menuBarDataTimeSpace
			// 
			this.menuBarDataTimeSpace.Name = "menuBarDataTimeSpace";
			this.menuBarDataTimeSpace.Size = new System.Drawing.Size(148, 22);
			this.menuBarDataTimeSpace.Text = "车辆时空图";
			this.menuBarDataTimeSpace.Click += new System.EventHandler(this.MenuBar_Data_TimeSpace_Click);
			// 
			// menuBarDataSpeedTime
			// 
			this.menuBarDataSpeedTime.Name = "menuBarDataSpeedTime";
			this.menuBarDataSpeedTime.Size = new System.Drawing.Size(148, 22);
			this.menuBarDataSpeedTime.Text = "速度时间图";
			this.menuBarDataSpeedTime.Click += new System.EventHandler(this.MenuBar_Data_SpeedTime_Click);
			// 
			// menuBarDataRoadMeanSpeed
			// 
			this.menuBarDataRoadMeanSpeed.Name = "menuBarDataRoadMeanSpeed";
			this.menuBarDataRoadMeanSpeed.Size = new System.Drawing.Size(148, 22);
			this.menuBarDataRoadMeanSpeed.Text = "路段平均速度";
			this.menuBarDataRoadMeanSpeed.Click += new System.EventHandler(this.MenuBar_Data_RoadMeanTime_Click);
			// 
			// menuBarDataOutput
			// 
			this.menuBarDataOutput.Name = "menuBarDataOutput";
			this.menuBarDataOutput.Size = new System.Drawing.Size(148, 22);
			this.menuBarDataOutput.Text = "仿真数据导出";
			this.menuBarDataOutput.Click += new System.EventHandler(this.MenuBar_Data_DataOutPut_Click);
			// 
			// 图表ToolStripMenuItem
			// 
			this.图表ToolStripMenuItem.Name = "图表ToolStripMenuItem";
			this.图表ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.图表ToolStripMenuItem.Text = "图表";
			// 
			// 关于ToolStripMenuItem
			// 
			this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.关于ToolStripMenuItem2,
			this.关于ToolStripMenuItem3,
			this.关于ToolStripMenuItem4});
			this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
			this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.关于ToolStripMenuItem.Text = "帮助";
			// 
			// 关于ToolStripMenuItem2
			// 
			this.关于ToolStripMenuItem2.Name = "关于ToolStripMenuItem2";
			this.关于ToolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
			this.关于ToolStripMenuItem2.Text = "访问作者";
			// 
			// 关于ToolStripMenuItem3
			// 
			this.关于ToolStripMenuItem3.Name = "关于ToolStripMenuItem3";
			this.关于ToolStripMenuItem3.Size = new System.Drawing.Size(121, 6);
			// 
			// 关于ToolStripMenuItem4
			// 
			this.关于ToolStripMenuItem4.Name = "关于ToolStripMenuItem4";
			this.关于ToolStripMenuItem4.Size = new System.Drawing.Size(124, 22);
			this.关于ToolStripMenuItem4.Text = "关于";
			this.关于ToolStripMenuItem4.Click += new System.EventHandler(this.MenuBar_Help_About_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripButtonNew,
			this.toolStripButtonOpen,
			this.toolStripButtonSave,
			this.toolStripSeparator1,
			this.toolStripButtonPointer,
			this.toolStripButtonPencil,
			this.toolStripButtonLine,
			this.toolStripButtonRectangle,
			this.toolStripButtonEllipse,
			this.toolStripSeparator2,
			this.toolStripButtonUndo,
			this.toolStripButtonRedo,
			this.toolStripSeparator3});
			this.toolStrip.Location = new System.Drawing.Point(0, 25);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(964, 27);
			this.toolStrip.TabIndex = 14;
			this.toolStrip.Text = "toolStrip1";
			// 
			// toolStripButtonNew
			// 
			this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
			this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonNew.Name = "toolStripButtonNew";
			this.toolStripButtonNew.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonNew.Text = "New";
			// 
			// toolStripButtonOpen
			// 
			this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
			this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonOpen.Name = "toolStripButtonOpen";
			this.toolStripButtonOpen.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonOpen.Text = "Open";
			this.toolStripButtonOpen.Click += new System.EventHandler(this.ToolStripButtonOpenClick);
			// 
			// toolStripButtonSave
			// 
			this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
			this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonSave.Name = "toolStripButtonSave";
			this.toolStripButtonSave.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonSave.Text = "Save";
			this.toolStripButtonSave.Click += new System.EventHandler(this.ToolStripButtonSaveClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
			// 
			// toolStripButtonPointer
			// 
			this.toolStripButtonPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPointer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPointer.Image")));
			this.toolStripButtonPointer.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonPointer.Name = "toolStripButtonPointer";
			this.toolStripButtonPointer.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonPointer.Text = "Pointer";
			// 
			// toolStripButtonPencil
			// 
			this.toolStripButtonPencil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPencil.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPencil.Image")));
			this.toolStripButtonPencil.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonPencil.Name = "toolStripButtonPencil";
			this.toolStripButtonPencil.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonPencil.Text = "Pencil";
			this.toolStripButtonPencil.Click += new System.EventHandler(this.ToolStripButtonPencilClick);
			// 
			// toolStripButtonLine
			// 
			this.toolStripButtonLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLine.Image")));
			this.toolStripButtonLine.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonLine.Name = "toolStripButtonLine";
			this.toolStripButtonLine.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonLine.Text = "Line";
			this.toolStripButtonLine.Click += new System.EventHandler(this.ToolStripButtonLineClick);
			// 
			// toolStripButtonRectangle
			// 
			this.toolStripButtonRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRectangle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRectangle.Image")));
			this.toolStripButtonRectangle.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonRectangle.Name = "toolStripButtonRectangle";
			this.toolStripButtonRectangle.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonRectangle.Text = "Rectangle";
			this.toolStripButtonRectangle.Click += new System.EventHandler(this.ToolStripButtonRectangleClick);
			// 
			// toolStripButtonEllipse
			// 
			this.toolStripButtonEllipse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonEllipse.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEllipse.Image")));
			this.toolStripButtonEllipse.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonEllipse.Name = "toolStripButtonEllipse";
			this.toolStripButtonEllipse.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonEllipse.Text = "Ellipse";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
			// 
			// toolStripButtonUndo
			// 
			this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndo.Image")));
			this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonUndo.Name = "toolStripButtonUndo";
			this.toolStripButtonUndo.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonUndo.Text = "Undo";
			this.toolStripButtonUndo.Click += new System.EventHandler(this.ToolStripButtonUndoClick);
			// 
			// toolStripButtonRedo
			// 
			this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRedo.Image")));
			this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Silver;
			this.toolStripButtonRedo.Name = "toolStripButtonRedo";
			this.toolStripButtonRedo.Size = new System.Drawing.Size(24, 24);
			this.toolStripButtonRedo.Text = "Redo";
			this.toolStripButtonRedo.Click += new System.EventHandler(this.ToolStripButtonRedoClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
			// 
			// drawArea
			// 
			this.drawArea.ActiveTool = SubSys_NetworkBuilder.DrawArea.DrawToolType.Pointer;
			this.drawArea.AutoScroll = true;
			this.drawArea.AutoSize = true;
			this.drawArea.BackColor = System.Drawing.Color.White;
			this.drawArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.drawArea.DocManager = null;
			this.drawArea.GraphicsList = null;
			this.drawArea.Location = new System.Drawing.Point(0, 52);
			this.drawArea.Name = "drawArea";
			this.drawArea.Owner = null;
			this.drawArea.Size = new System.Drawing.Size(964, 461);
			this.drawArea.TabIndex = 15;
			this.drawArea.Load += new System.EventHandler(this.drawArea_Load);
			// 
			// 撤销UToolStripMenuItem
			// 
			this.撤销UToolStripMenuItem.Name = "撤销UToolStripMenuItem";
			this.撤销UToolStripMenuItem.Size = new System.Drawing.Size(149, 6);
			// 
			// SimMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(964, 535);
			this.Controls.Add(this.drawArea);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.menuBar);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuBar;
			this.Name = "SimMain";
			this.Text = "TrafficSim交通仿真程序";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.SimMainLoad);
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.menuBar.ResumeLayout(false);
			this.menuBar.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		
		private ToolStripMenuItem MenuBarConfigFormBackColor;
		private ToolStripMenuItem MenuBarConfigParameterSet;
		private ToolStripMenuItem MenubarSimlateStop;
		private ToolStripMenuItem menuBarPrgExit;
		
		private ToolStripMenuItem menuBarDataSpeedTime;
		private ToolStripMenuItem menuBarDataRoadMeanSpeed;
		private ToolStripMenuItem menuBarDataTimeSpace;
		private ToolStripMenuItem menuBarDataOutput;
		private MenuStrip menuBar;
		private ToolStripMenuItem menuBarFile;
		private ToolStripMenuItem menuBarFileCreateNetwork;
		private ToolStripMenuItem menuBarFileSaveNetWork;
		private ToolStripMenuItem menuBarEdit;
		private ToolStripMenuItem menuBarEditRoadNetwork;
		private ToolStripMenuItem menuBarConfig;
		private ToolStripMenuItem menuBarConfigSimEnvr;
		
		private ToolStripMenuItem menuBarSimulate;
		private ToolStripMenuItem menuBarSimulateSustained;
		private ToolStripMenuItem menuBarSimulateRunSingleStep;
		private ToolStripMenuItem menuBarSimulatePause;
		private ToolStripMenuItem menuBarSimulateResume;
		
		
		private ToolStripMenuItem menuBarData;
		private ToolStripMenuItem 图表ToolStripMenuItem;

		#endregion

		//private Button button2;
		private StatusStrip statusBar;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripStatusLabel tslSimTime;
		private ToolTip tpMousePositonTip;
		private ToolStripSplitButton toolStripSplitButton1;
		private ToolStripStatusLabel tsslMsgTip;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStrip toolStrip;
		private ToolStripButton toolStripButtonNew;
		private ToolStripButton toolStripButtonOpen;
		private ToolStripButton toolStripButtonSave;
		private ToolStripButton toolStripButtonPointer;
		private ToolStripButton toolStripButtonRectangle;
		private ToolStripButton toolStripButtonEllipse;
		private ToolStripButton toolStripButtonLine;
		private ToolStripButton toolStripButtonPencil;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton toolStripButtonUndo;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripButton toolStripButtonRedo;
		private DrawArea drawArea;
		private ToolStripMenuItem 关于ToolStripMenuItem;
		private ToolStripMenuItem 关于ToolStripMenuItem2;
		private ToolStripSeparator 关于ToolStripMenuItem3;
		private ToolStripMenuItem 关于ToolStripMenuItem4;
		private System.Windows.Forms.ToolStripSeparator 撤销UToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 重做ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 重做RToolStripMenuItem;
		
		
	  
	}
}