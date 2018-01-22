using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using System;
using System.Windows.Forms;


namespace SubSys_NetworkBuilder
{
    public partial class WaySetter : Form
    {
        internal WaySetter()
        {
            InitializeComponent();
        }

        private void BTConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void BTCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void WaySetter_Load(object sender, EventArgs e)
        {
            this.TBWayNumber.Text = TrafficOBJ.EntityCounter.ToString();
        }
    }
}
