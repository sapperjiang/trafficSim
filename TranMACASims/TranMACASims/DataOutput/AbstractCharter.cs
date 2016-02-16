using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSys_DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace GISTranSim.Data
{
    public partial class AbstractCharter : System.Windows.Forms.Form
    {
        protected AbstractCharter()
        {
//            InitializeComponent();
        }

        protected ChartDataProvider DataProvider;

        protected void Chart(ChartDataProvider cdp, Chart CHART_SpaceTime)
        {
            this.DataProvider = cdp;
            CHART_SpaceTime.ChartAreas.Add(new ChartArea("TimeSpace"));
            ChartArea st = CHART_SpaceTime.ChartAreas[0];

            st.AxisX.MajorGrid.Enabled = false;
            st.AxisY.MajorGrid.Enabled = false;

            //DataProvider = new TimeSpaceDataProvider();
            this.Text = DataProvider.strHeaderTitle;
            st.AxisX.Title = DataProvider.strXAiexTitle;
            st.AxisY.Title = DataProvider.strYAiexTitle;
            st.AxisX2.Title = DataProvider.strHeaderTitle;

            DataProvider.FillSerisCollection(CHART_SpaceTime.Series);

            CHART_SpaceTime.Show();
        }
    }
}
