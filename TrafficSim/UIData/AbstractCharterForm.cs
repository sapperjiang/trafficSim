using System.Windows.Forms.DataVisualization;
using SubSys_DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace TrafficSim
{
    public partial class AbstractCharterForm : System.Windows.Forms.Form
    {
        protected AbstractCharterForm()
        {
            this.TopLevel = false;
            //            InitializeComponent();
        }

        protected DataCharter charter;

        public void Chart(DataCharter chartor, Chart CHART)
        {
            this.charter = chartor;
            CHART.ChartAreas.Clear();// Add(new ChartArea("TimeSpace"));

            CHART.ChartAreas.Add(new ChartArea("TimeSpace"));
            ChartArea st = CHART.ChartAreas[0];

            st.AxisX.MajorGrid.Enabled = false;
            st.AxisY.MajorGrid.Enabled = false;

            this.Text = charter.strHeaderTitle;
            st.AxisX.Title = charter.strXAiexTitle;
            st.AxisY.Title = charter.strYAiexTitle;
            st.AxisX2.Title = charter.strHeaderTitle;

            charter.FillSerisCollection(CHART.Series);

            CHART.Show();
        }


        public virtual void DrawChart() { }
    
    }
}
