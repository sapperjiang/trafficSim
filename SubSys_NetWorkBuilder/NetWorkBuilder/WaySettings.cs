using System;
using System.Windows.Forms;


namespace SubSys_NetworkBuilder
{
    public partial class SJUI_WaySettings : Form
    {
       // public Way way;
        internal SJUI_WaySettings()
        {
            InitializeComponent();
        }
        //public FormWaySettings(Way way)
        //{
        //   InitializeComponent();
        ////   this.TBWayNumber.Text = way.ID.ToString();
        //}

        private void BTConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void BTCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
