using System;
using System.Collections.Generic;
using System.Text;
using GISTranSim;
using MapWindow.Interfaces;
using SubSys_SimDriving;
using GISTranSim.Data;


namespace PluginTest
{
    using System;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;
    using MapWindow.Interfaces;
    using SubSys_SimDriving.TrafficModel;
//    using System.Windows.Forms.ComponentModel;
//using System.bupiao
   
  
/// <summary>
/// 用于仿真软件编写在MapWindow框架下的插件使用。Every plug-in must have a class that implements IPlugin.The class that implements this interface *MUST* be unique,or MapWindow will think it's an updated/different version of an existing plugin.
/// </summary>
    public class TranSimGISPlugin : MapWindow.Interfaces.IPlugin
    {
        // Change this to match the name of your class. This is the constructor.
        public TranSimGISPlugin()
        {
        }
        private Control convas;
        private IMapWin mMapWin;
        private string strBTNConfig = "仿真配置";
        private string strBTNRun = "仿真运行";
        private string strBTNPause = "仿真停止";

        private string strBTNPanNetwork = "移动路网";
        private string strBTNSpaceTime = "时空图像";
        private string strBTNSpeedTime = "速度时间";
        private string strBTNMeanSpeed = "平均速度";
        private string strBTNShowData = "显示数据";
        private string strToolBar = "GISTranToolBar";
        //private readonly Stack addedButtons = new Stack();
        ToolbarButton TBTN_Config;
        ToolbarButton TBTN_Run;
        ToolbarButton TBTN_Pause;
        ToolbarButton TBTN_PanNetwork;
        ToolbarButton TBTN_ChartSpeedTime;
        ToolbarButton TBTN_ChartSpaceTime;
        ToolbarButton TBTN_ChartMeanSpeed;
        ToolbarButton TBTN_ShowData;
        SimMain scConvas; 

        //This event is fired when the user loads your plug-in either through the plug-in dialog 
        //box, or by checkmarking it in the plug-ins menu.  This is where you would add buttons to the
        //tool bar or menu items to the menu.  
        //It is also standard to set a global reference to the IMapWin that is passed through here so that
        //you can access it elsewhere in your project to act on MapWindow.
        public void Initialize(MapWindow.Interfaces.IMapWin m_MapWin, int ParentHandle)
        {
            this.mMapWin = m_MapWin;
            this.convas = (Form)Control.FromHandle(new IntPtr(ParentHandle));
            //conv
            ResourceManager res = new ResourceManager("GISTranSim.Properties.Resources", Assembly.GetExecutingAssembly());
            Toolbar toolbar = mMapWin.Toolbar;;
            toolbar.AddToolbar(strToolBar);
            if (strToolBar.Length > 0)
            {
                toolbar.AddToolbar(strToolBar);
            }
           
            TBTN_Config = toolbar.AddButton(strBTNConfig, strToolBar, string.Empty, string.Empty);
            TBTN_Config.Tooltip = "配置仿真应用程序";
            TBTN_Config.Category = strToolBar;
            TBTN_Config.Picture = res.GetObject("Config");
            TBTN_Config.Enabled = true;

            TBTN_Config.Text = strBTNConfig; //this.res.GetString("textCreateShp");

            TBTN_Run = toolbar.AddButton(strBTNRun, strToolBar, false);
            TBTN_Run.Category = strToolBar;
            TBTN_Run.Text = strBTNRun;
            TBTN_Run.Tooltip = "运行仿真应用程序";
            TBTN_Run.Enabled = false;
            TBTN_Run.Picture = res.GetObject("Run");

      
            TBTN_Pause = toolbar.AddButton(strBTNPause, strToolBar, false);
            TBTN_Pause.Category = strToolBar;
            TBTN_Pause.Text = strBTNPause;
            TBTN_Pause.Tooltip = "停止仿真程序";
            TBTN_Pause.Enabled = false;
            TBTN_Pause.Picture = res.GetObject("Pause");


            TBTN_ChartSpeedTime = toolbar.AddButton(strBTNSpaceTime, strToolBar, false);
            TBTN_ChartSpeedTime.Category = strToolBar;
            TBTN_ChartSpeedTime.Text = strBTNSpaceTime;
            TBTN_ChartSpeedTime.Tooltip = "输出时空图像";
            TBTN_ChartSpeedTime.Enabled = false;
            TBTN_ChartSpeedTime.Picture = res.GetObject("Chart2");


            TBTN_ChartSpaceTime = toolbar.AddButton(strBTNSpeedTime, strToolBar, false);
            TBTN_ChartSpaceTime.Category = strToolBar;
            TBTN_ChartSpaceTime.Text = strBTNSpeedTime;
            TBTN_ChartSpaceTime.Tooltip = "输出速度时间图像";
            TBTN_ChartSpaceTime.Enabled = false;
            TBTN_ChartSpaceTime.Picture = res.GetObject("Chart1");


            TBTN_ChartMeanSpeed = toolbar.AddButton(strBTNMeanSpeed, strToolBar, false);
            TBTN_ChartMeanSpeed.Category = strToolBar;
            TBTN_ChartMeanSpeed.Text = strBTNSpeedTime;
            TBTN_ChartMeanSpeed.Tooltip = "输出平均速度图像";
            TBTN_ChartMeanSpeed.Enabled = false;
            TBTN_ChartMeanSpeed.Picture = res.GetObject("Save");
            
            TBTN_ShowData = toolbar.AddButton(strBTNShowData, strToolBar, false);
            TBTN_ShowData.Category = strToolBar;
            TBTN_ShowData.Text = strBTNShowData;
            TBTN_ShowData.Tooltip = "输出仿真数据";
            TBTN_ShowData.Enabled = false;
            TBTN_ShowData.Picture = res.GetObject("Data");

        }

        //This is a description of the plug-in.  It appears in the plug-ins dialog box when a user selects
        //your plug-in.
        public string Description
        {
            get
            {
                return " a simple traffic simulation plugin. sapperjiang@qq.com";
            }
        }

        //This is the author of the plug-in. It can be a company name, individual, or organization name.
        public string Author
        {
            get
            {
                return "Sapperjiang china shanghai";
            }
        }

        //This is one of the more important plug-in properties because if it is not set to something then
        //your plug-in will not load at all. This is the name that appears in the Plug-ins menu to identify
        //this plug-in.
        public string Name
        {
            get
            {
                return "GisTrafficSim";
            }
        }

        //This is the version number of the plug-in.  You can either return a hard-coded string
        //such as "1.0.0.1" or you can use the .NET function shown below to dynamically return 
        //the version number from the assembly itself.
        public string Version
        {
            get
            {
                return "1.0.0000";
            }
        }

        //This is the Build Date for the plug-in.  You can either return a string of a hard-coded date
        //such as "January 1, 2003" or you can use the .NET function below to dynamically obtain the build
        //date of the assembly.
        public string BuildDate
        {
            get
            {
                return System.DateTime.Now.ToString();
            }
        }

        //When the user opens a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        //project that the user opened (including its path in case that is important for this this plug-in to know).
        //The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        //on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        //separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        //one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        //SettingsString of the project. 
        public void ProjectLoading(string ProjectFile, string SettingsString)
        {
        }

        //Plug-ins can communicate with eachother using Messages.  If a message is sent then this event fires.
        //If you know the message is "for you" then you can set Handled=True and then it will not be sent to any
        //other plug-ins.
        public void Message(string msg, ref bool Handled)
        {
        }

        //This event fires any time there is a zoom or pan that changes the extents of the map.
        public void MapExtentsChanged()
        {
        }

        //This event is fired when the user unloads your plug-in either through the plug-in dialog 
        //box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any
        //buttons from the tool bar tool bar or menu items from the menu that you may have added.
        //If you don't do this, then you will leave dangling menus and buttons that don't do anything.
        public void Terminate()
        {
            SimController.bIsExit = true;
            mMapWin.Toolbar.RemoveButton(TBTN_Config.Name);
            mMapWin.Toolbar.RemoveButton(TBTN_Pause.Name);
            mMapWin.Toolbar.RemoveButton(TBTN_Run.Name);
            mMapWin.Toolbar.RemoveToolbar(strToolBar);
            if (this.scConvas!=null)
            {
                scConvas.Dispose();
            }
        }

        //This event fires when the user holds a mouse button down on the map. Note that x and y are returned
        //as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        //then you need to use g_MapWin.View.PixelToProj()
        public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {

        }

        //When the user saves a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        //project that the user is saving (including its path in case that is important for this this plug-in to know).
        //The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        //on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        //separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        //one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        //SettingsString of the project.
        public void ProjectSaving(string ProjectFile, ref string SettingsString)
        {
        }

        //This event fires when a menu item or toolbar button is clicked.  So if you added a button or menu
        //on the Initialize event, then this is where you would handle it.
        public void ItemClicked(string ItemName, ref bool Handled)
        {
            if (ItemName == this.strBTNRun)
            {
                //显示仿真窗口
                //scConvas.Show();
                this.convas.Show();
                SimController.Start();
                Handled = true;
                TBTN_Config.Enabled = false;
                TBTN_Pause.Enabled = true;

                TBTN_ChartMeanSpeed.Enabled = false;
                TBTN_ChartSpaceTime.Enabled = false;
                TBTN_ChartSpeedTime.Enabled = false;
                TBTN_ShowData.Enabled = false;


            }
         
            if (ItemName == this.strBTNConfig)
            {
            
                TBTN_Run.Enabled = true;
                TBTN_Pause.Enabled = true;
                this.convas = this.mMapWin.UIPanel.CreatePanel("TranSimGIS", MapWindowDockStyle.Right);


                GISTranSim.SimConfig cs = new GISTranSim.SimConfig();
                if (cs.ShowDialog() == DialogResult.OK)
                {
                    SimController.iCarCount = cs.iCarCount;
                    SimController.iRoadWidth = cs.iRoadLength;
                    SimController.iSimInterval = cs.iSimSpeed;
                    ModelSetting.dRate = cs.dRatio;

                } cs.Dispose();

             
                SimController.ConfigSimEnvironment(this.convas);
                TBTN_Config.Enabled = false;
                TBTN_Pause.Enabled = true;

                Handled = true;
            }
           
            //暂停
            if (ItemName == this.strBTNPause)
            {
                //SimController.IsPause = true;
                SimController.bIsExit = true;

                TBTN_Run.Enabled = true;

                TBTN_ChartMeanSpeed.Enabled = true;
                TBTN_ChartSpaceTime.Enabled = true;
                TBTN_ChartSpeedTime.Enabled = true;
                TBTN_ShowData.Enabled = true;

                TBTN_Config.Enabled = false;
                Handled = true;

            }
            //平均速度
            if (ItemName == this.strBTNMeanSpeed)
            {
                MeanSpeedCharter msc = new MeanSpeedCharter();
                msc.Show();
                Handled = true;

            }
            //时间空间图
            if (ItemName == this.strBTNSpaceTime)
            {
                TimeSpaceCharter tsc = new TimeSpaceCharter();
                tsc.Show();
                Handled = true;

            }
            //速度时间图
            if (ItemName == this.strBTNSpeedTime)
            {
                SpeedTimeCharter st = new SpeedTimeCharter();
                st.Show();
                Handled = true;

            }

            //速度时间图
            if (ItemName == this.strBTNShowData)
            {
                DataOutputer dop = new DataOutputer();
                dop.Show();
                Handled = true;

            }
        }

        //This event fires when the user adds a layer to MapWindow.  This is useful to know if your
        //plug-in depends on a particular layer being present. Also, if you keep an internal list of 
        //available layers, for example you may be keeping a list of all "point" shapefiles, then you
        //would use this event to know when layers have been added or removed.
        public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)
        {
        }

        //This event fires when a user selects a layer in the legend. 
        public void LayerSelected(int Handle)
        {
        }

        //If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag
        //and returns a system.drawing.rectangle that has the bounds of the box that was "drawn"
        public void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled)
        {
        }

        //This event fires when the user releases a mouse button down on the map. Note that x and y are returned
        //as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        //then you need to use g_MapWin.View.PixelToProj()
        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
        }

        //This event fires when a user double-clicks a layer in the legend.
        public void LegendDoubleClick(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {
        }

        //This event fires when a user holds a mouse button down in the legend.
        public void LegendMouseDown(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {
        }

        //This event fires when a user releases a mouse button in the legend.
        public void LegendMouseUp(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {
        }

        //This is the plug-in serial number. 
        //This is no longer needed, but the property must remain for backward compatibility.
        public string SerialNumber
        {
            get
            {
                // This is unnecessary but the implementation must be here for backward compatibility.
                return null;
            }
        }

        //This event fires when the user removes a layer from MapWindow.  This is useful to know if your
        //plug-in depends on a particular layer being present. 
        public void LayerRemoved(int Handle)
        {
        }

        //This event fires when the user moves the mouse over the map. Note that x and y are returned
        //as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        //then you need to use g_MapWin.View.PixelToProj()
        public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            //this.mMapWin.View.PixelToProj(
        }

        //This event fires when the user clears all of the layers from MapWindow.  As with LayersAdded 
        //and LayersRemoved, this is useful to know if your plug-in depends on a particular layer being 
        //present or if you are maintaining your own list of layers.
        public void LayersCleared()
        {
        }

        //This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the 
        //Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the 
        //shapes that were selected. 
        public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
        {
        }
    }

}
