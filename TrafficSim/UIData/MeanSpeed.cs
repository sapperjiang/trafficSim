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
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;
using SubSys_DataVisualization;


namespace TrafficSim
{
    public partial class MeanSpeed: AbstractCharterForm
    {
        public MeanSpeed()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.Chart(new SubSys_DataVisualization.MeanSpeedCharter(), CHART_SpaceTime);
            base.OnShown(e);
        }

        public override void DrawChart()
        {
            base.Chart(new SubSys_DataVisualization.MeanSpeedCharter(), CHART_SpaceTime);
        }


    }
}
