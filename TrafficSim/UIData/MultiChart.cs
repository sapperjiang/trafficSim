using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SubSys_DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace TrafficSim
{
    public partial class MultiChart : Form
    {
        public MultiChart()
        {
            InitializeComponent();
            chart1.ChartAreas.Clear();
            charter = new TimeSpaceCharter();
            this.Text = charter.strHeaderTitle;

            chart1.ChartAreas.Add(new ChartArea("TimeSpace"));
            chart2.ChartAreas.Add(new ChartArea("TimeSpeed"));
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
        }
        DataCharter charter;
        public void DrawMultiChart()
        {
            charter = new TimeSpaceCharter();
            ChartArea st = chart1.ChartAreas["TimeSpace"];

            st.AxisX.MajorGrid.Enabled = false;
            st.AxisY.MajorGrid.Enabled = false;

            st.AxisX.Title = charter.strXAiexTitle;
            st.AxisY.Title = charter.strYAiexTitle;
            st.AxisX2.Title = charter.strHeaderTitle;

            charter.FillSerisCollection(chart1.Series);


            charter = new TimeSpeedCharter();
            st = chart2.ChartAreas["TimeSpeed"];
            st.AxisX.MajorGrid.Enabled = false;
            st.AxisY.MajorGrid.Enabled = false;

            st.AxisX.Title = charter.strXAiexTitle;
            st.AxisY.Title = charter.strYAiexTitle;
            st.AxisX2.Title = charter.strHeaderTitle;
            charter.FillSerisCollection(chart2.Series);

        }

    }
}
