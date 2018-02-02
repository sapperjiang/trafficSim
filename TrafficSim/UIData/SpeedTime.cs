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


namespace TrafficSim
{
    public partial class SpeedTime : AbstractCharterForm
    {
        public SpeedTime()
        {
            InitializeComponent();
        }
        protected override void OnShown(EventArgs e)
        {
            DrawChart();

            base.OnShown(e);
        }

        public override void DrawChart()
        {//for updating
            CHART_SpaceTime.ChartAreas.Clear();//.Add(new ChartArea("TimeSpace"));

            CHART_SpaceTime.ChartAreas.Add(new ChartArea("TimeSpace"));
            SeriesCollection dataSRC = CHART_SpaceTime.Series;
            ChartArea st = CHART_SpaceTime.ChartAreas[0];

            st.AxisX.MajorGrid.Enabled = false;
            st.AxisY.MajorGrid.Enabled = false;

            st.AxisY.Title = "速度(m/s)";
            st.AxisX.Title = "时间(s)";

            ISimContext ISC = SimContext.GetInstance();
            int iSpeed;
            foreach (IDataRecorder<int, CarTrack> itemEntity in ISC.DataRecorder.Values)
            {
                foreach (KeyValuePair<int, CarTrack> item in itemEntity)//carinfo Queue
                {
                    Series dataI = dataSRC.FindByName(item.Key.ToString());

                    //同一辆车在不同的位置
                    if (dataI == null)
                    {
                        dataI = new Series(item.Key.ToString());
                        dataI.MarkerStyle = MarkerStyle.Diamond;
                        dataI.ChartType = SeriesChartType.Line;
                        dataSRC.Add(dataI);
                    }

                    foreach (var itemCarInfo in item.Value)//车辆信息
                    {
                        iSpeed = itemCarInfo.iSpeed * SimSettings.iCellWidth;
                        dataI.Points.AddXY(itemCarInfo.iTimeStep, iSpeed);
                    }
                }
            }

            CHART_SpaceTime.Show();
        }

    }
}
