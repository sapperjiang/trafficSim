using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;
using SubSys_DataVisualization;


namespace GISTranSim.Data
{
    public partial class TimeSpaceCharter : AbstractCharterForm
    {
        public TimeSpaceCharter()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.Chart(new TimeSpaceDataProvider(), CHART_SpaceTime);
            base.OnShown(e);
        }

       
    }
}
