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
        	this.TSSL_MsgTip = new System.Windows.Forms.ToolStripStatusLabel();
        	this.TP_MousePos = new System.Windows.Forms.ToolTip(this.components);
        	this.menuBar = new System.Windows.Forms.MenuStrip();
        	this.menuBarFile = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarFileCreateNetwork = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarFileSaveNetWork = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarPrgExit = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarEdit = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarEditRoadNetwork = new System.Windows.Forms.ToolStripMenuItem();
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
        	this.menuBarDataSpeedTime = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarDataRoadMeanSpeed = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarDataTimeSpace = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuBarDataOutput = new System.Windows.Forms.ToolStripMenuItem();
        	this.图表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.statusBar.SuspendLayout();
        	this.menuBar.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// statusBar
        	// 
        	this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripStatusLabel1,
			this.tslSimTime,
			this.toolStripSplitButton1,
			this.TSSL_MsgTip});
        	this.statusBar.Location = new System.Drawing.Point(0, 458);
        	this.statusBar.Name = "statusBar";
        	this.statusBar.Size = new System.Drawing.Size(964, 27);
        	this.statusBar.TabIndex = 3;
        	this.statusBar.Text = "statusStrip1";
        	// 
        	// toolStripStatusLabel1
        	// 
        	this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        	this.toolStripStatusLabel1.Size = new System.Drawing.Size(90, 22);
        	this.toolStripStatusLabel1.Text = "当前时间：";
        	// 
        	// tslSimTime
        	// 
        	this.tslSimTime.Name = "tslSimTime";
        	this.tslSimTime.Size = new System.Drawing.Size(20, 22);
        	this.tslSimTime.Text = "0";
        	// 
        	// toolStripSplitButton1
        	// 
        	this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        	this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
        	this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.toolStripSplitButton1.Name = "toolStripSplitButton1";
        	this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 25);
        	this.toolStripSplitButton1.Text = "toolStripSplitButton1";
        	// 
        	// TSSL_MsgTip
        	// 
        	this.TSSL_MsgTip.Name = "TSSL_MsgTip";
        	this.TSSL_MsgTip.Size = new System.Drawing.Size(0, 22);
        	// 
        	// TP_MousePos
        	// 
        	this.TP_MousePos.AutoPopDelay = 5000;
        	this.TP_MousePos.InitialDelay = 500;
        	this.TP_MousePos.ReshowDelay = 100;
        	// 
        	// menuBar
        	// 
        	this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarFile,
			this.menuBarEdit,
			this.menuBarConfig,
			this.menuBarSimulate,
			this.menuBarData,
			this.图表ToolStripMenuItem});
        	this.menuBar.Location = new System.Drawing.Point(0, 0);
        	this.menuBar.Name = "menuBar";
        	this.menuBar.Size = new System.Drawing.Size(964, 30);
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
        	this.menuBarFile.Size = new System.Drawing.Size(54, 26);
        	this.menuBarFile.Text = "文件";
        	// 
        	// menuBarFileCreateNetwork
        	// 
        	this.menuBarFileCreateNetwork.Name = "menuBarFileCreateNetwork";
        	this.menuBarFileCreateNetwork.Size = new System.Drawing.Size(144, 26);
        	this.menuBarFileCreateNetwork.Text = "新建路网";
        	this.menuBarFileCreateNetwork.Click += new System.EventHandler(this.MemuBar_File_CreateNetWork_Click);
        	// 
        	// menuBarFileSaveNetWork
        	// 
        	this.menuBarFileSaveNetWork.Enabled = false;
        	this.menuBarFileSaveNetWork.Name = "menuBarFileSaveNetWork";
        	this.menuBarFileSaveNetWork.Size = new System.Drawing.Size(144, 26);
        	this.menuBarFileSaveNetWork.Text = "保存路网";
        	// 
        	// menuBarPrgExit
        	// 
        	this.menuBarPrgExit.Name = "menuBarPrgExit";
        	this.menuBarPrgExit.Size = new System.Drawing.Size(144, 26);
        	this.menuBarPrgExit.Text = "退出";
        	// 
        	// menuBarEdit
        	// 
        	this.menuBarEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarEditRoadNetwork});
        	this.menuBarEdit.Name = "menuBarEdit";
        	this.menuBarEdit.Size = new System.Drawing.Size(54, 26);
        	this.menuBarEdit.Text = "编辑";
        	// 
        	// menuBarEditRoadNetwork
        	// 
        	this.menuBarEditRoadNetwork.Enabled = false;
        	this.menuBarEditRoadNetwork.Name = "menuBarEditRoadNetwork";
        	this.menuBarEditRoadNetwork.Size = new System.Drawing.Size(144, 26);
        	this.menuBarEditRoadNetwork.Text = "编辑路网";
        	// 
        	// menuBarConfig
        	// 
        	this.menuBarConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarConfigSimEnvr,
			this.MenuBarConfigParameterSet,
			this.MenuBarConfigFormBackColor});
        	this.menuBarConfig.Name = "menuBarConfig";
        	this.menuBarConfig.Size = new System.Drawing.Size(54, 26);
        	this.menuBarConfig.Text = "配置";
        	// 
        	// menuBarConfigSimEnvr
        	// 
        	this.menuBarConfigSimEnvr.Name = "menuBarConfigSimEnvr";
        	this.menuBarConfigSimEnvr.Size = new System.Drawing.Size(144, 26);
        	this.menuBarConfigSimEnvr.Text = "加载路网";
        	this.menuBarConfigSimEnvr.Click += new System.EventHandler(this.MenuBar_File_ConfigEnvr_Click);
        	// 
        	// MenuBarConfigParameterSet
        	// 
        	this.MenuBarConfigParameterSet.Enabled = false;
        	this.MenuBarConfigParameterSet.Name = "MenuBarConfigParameterSet";
        	this.MenuBarConfigParameterSet.Size = new System.Drawing.Size(144, 26);
        	this.MenuBarConfigParameterSet.Text = "参数设置";
        	this.MenuBarConfigParameterSet.Click += new System.EventHandler(this.MenuBar_Config_Parameter_Click);
        	// 
        	// MenuBarConfigFormBackColor
        	// 
        	this.MenuBarConfigFormBackColor.Name = "MenuBarConfigFormBackColor";
        	this.MenuBarConfigFormBackColor.Size = new System.Drawing.Size(144, 26);
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
        	this.menuBarSimulate.Size = new System.Drawing.Size(54, 26);
        	this.menuBarSimulate.Text = "运行";
        	// 
        	// menuBarSimulateSustained
        	// 
        	this.menuBarSimulateSustained.Enabled = false;
        	this.menuBarSimulateSustained.Name = "menuBarSimulateSustained";
        	this.menuBarSimulateSustained.Size = new System.Drawing.Size(144, 26);
        	this.menuBarSimulateSustained.Text = "启动仿真";
        	this.menuBarSimulateSustained.Click += new System.EventHandler(this.MenuBar_SimluateSustained_Click);
        	// 
        	// menuBarSimulateRunSingleStep
        	// 
        	this.menuBarSimulateRunSingleStep.Enabled = false;
        	this.menuBarSimulateRunSingleStep.Name = "menuBarSimulateRunSingleStep";
        	this.menuBarSimulateRunSingleStep.Size = new System.Drawing.Size(144, 26);
        	this.menuBarSimulateRunSingleStep.Text = "单步运行";
        	// 
        	// menuBarSimulatePause
        	// 
        	this.menuBarSimulatePause.Name = "menuBarSimulatePause";
        	this.menuBarSimulatePause.Size = new System.Drawing.Size(144, 26);
        	this.menuBarSimulatePause.Text = "暂停仿真";
        	this.menuBarSimulatePause.Click += new System.EventHandler(this.MenuBar_Simulate_Pause_Click);
        	// 
        	// menuBarSimulateResume
        	// 
        	this.menuBarSimulateResume.Name = "menuBarSimulateResume";
        	this.menuBarSimulateResume.Size = new System.Drawing.Size(144, 26);
        	this.menuBarSimulateResume.Text = "恢复仿真";
        	this.menuBarSimulateResume.Click += new System.EventHandler(this.MenuBar_Simulate_Resume_Click);
        	// 
        	// MenubarSimlateStop
        	// 
        	this.MenubarSimlateStop.Name = "MenubarSimlateStop";
        	this.MenubarSimlateStop.Size = new System.Drawing.Size(144, 26);
        	this.MenubarSimlateStop.Text = "结束仿真";
        	this.MenubarSimlateStop.Click += new System.EventHandler(this.Menubar_Simlate_Stop_Click);
        	// 
        	// menuBarData
        	// 
        	this.menuBarData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.menuBarDataSpeedTime,
			this.menuBarDataRoadMeanSpeed,
			this.menuBarDataTimeSpace,
			this.menuBarDataOutput});
        	this.menuBarData.Name = "menuBarData";
        	this.menuBarData.Size = new System.Drawing.Size(54, 26);
        	this.menuBarData.Text = "数据";
        	// 
        	// menuBarDataSpeedTime
        	// 
        	this.menuBarDataSpeedTime.Name = "menuBarDataSpeedTime";
        	this.menuBarDataSpeedTime.Size = new System.Drawing.Size(176, 26);
        	this.menuBarDataSpeedTime.Text = "速度时间图";
        	this.menuBarDataSpeedTime.Click += new System.EventHandler(this.MenuBar_Data_SpeedTime_Click);
        	// 
        	// menuBarDataRoadMeanSpeed
        	// 
        	this.menuBarDataRoadMeanSpeed.Name = "menuBarDataRoadMeanSpeed";
        	this.menuBarDataRoadMeanSpeed.Size = new System.Drawing.Size(176, 26);
        	this.menuBarDataRoadMeanSpeed.Text = "路段平均速度";
        	this.menuBarDataRoadMeanSpeed.Click += new System.EventHandler(this.MenuBar_Data_RoadMeanTime_Click);
        	// 
        	// menuBarDataTimeSpace
        	// 
        	this.menuBarDataTimeSpace.Name = "menuBarDataTimeSpace";
        	this.menuBarDataTimeSpace.Size = new System.Drawing.Size(176, 26);
        	this.menuBarDataTimeSpace.Text = "车辆时空图";
        	this.menuBarDataTimeSpace.Click += new System.EventHandler(this.MenuBar_Data_TimeSpace_Click);
        	// 
        	// menuBarDataOutput
        	// 
        	this.menuBarDataOutput.Name = "menuBarDataOutput";
        	this.menuBarDataOutput.Size = new System.Drawing.Size(176, 26);
        	this.menuBarDataOutput.Text = "仿真数据导出";
        	this.menuBarDataOutput.Click += new System.EventHandler(this.MenuBar_Data_DataOutPut_Click);
        	// 
        	// 图表ToolStripMenuItem
        	// 
        	this.图表ToolStripMenuItem.Name = "图表ToolStripMenuItem";
        	this.图表ToolStripMenuItem.Size = new System.Drawing.Size(54, 26);
        	this.图表ToolStripMenuItem.Text = "图表";
        	// 
        	// SimMain
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.SystemColors.Control;
        	this.ClientSize = new System.Drawing.Size(964, 485);
        	this.Controls.Add(this.statusBar);
        	this.Controls.Add(this.menuBar);
        	this.DoubleBuffered = true;
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MainMenuStrip = this.menuBar;
        	this.Name = "SimMain";
        	this.Text = "TrafficSim交通仿真程序";
        	this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        	this.statusBar.ResumeLayout(false);
        	this.statusBar.PerformLayout();
        	this.menuBar.ResumeLayout(false);
        	this.menuBar.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.ToolStripMenuItem MenuBarConfigFormBackColor;
        private System.Windows.Forms.ToolStripMenuItem MenuBarConfigParameterSet;
        private System.Windows.Forms.ToolStripMenuItem MenubarSimlateStop;
        private System.Windows.Forms.ToolStripMenuItem menuBarPrgExit;
        
        private System.Windows.Forms.ToolStripMenuItem menuBarDataSpeedTime;
        private System.Windows.Forms.ToolStripMenuItem menuBarDataRoadMeanSpeed;
        private System.Windows.Forms.ToolStripMenuItem menuBarDataTimeSpace;
        private System.Windows.Forms.ToolStripMenuItem menuBarDataOutput;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem menuBarFile;
        private System.Windows.Forms.ToolStripMenuItem menuBarFileCreateNetwork;
        private System.Windows.Forms.ToolStripMenuItem menuBarFileSaveNetWork;
        private System.Windows.Forms.ToolStripMenuItem menuBarEdit;
        private System.Windows.Forms.ToolStripMenuItem menuBarEditRoadNetwork;
        private System.Windows.Forms.ToolStripMenuItem menuBarConfig;
        private System.Windows.Forms.ToolStripMenuItem menuBarConfigSimEnvr;
        
        private System.Windows.Forms.ToolStripMenuItem menuBarSimulate;
        private System.Windows.Forms.ToolStripMenuItem menuBarSimulateSustained;
        private System.Windows.Forms.ToolStripMenuItem menuBarSimulateRunSingleStep;
        private System.Windows.Forms.ToolStripMenuItem menuBarSimulatePause;
        private System.Windows.Forms.ToolStripMenuItem menuBarSimulateResume;
        
        
        private System.Windows.Forms.ToolStripMenuItem menuBarData;
        private System.Windows.Forms.ToolStripMenuItem 图表ToolStripMenuItem;

        #endregion

        //private System.Windows.Forms.Button button2;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tslSimTime;
        private System.Windows.Forms.ToolTip TP_MousePos;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripStatusLabel TSSL_MsgTip;
    }
}