using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GISTranSim
{
    partial class UIHelpAbout : Form
    {
        public UIHelpAbout()
        {
            InitializeComponent();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            this.Text = "About " + Application.ProductName;

            var strMsg = "Program: " + Application.ProductName + "\n" +
                "Version: " + Application.ProductVersion;
            strMsg+=String.Concat("\n","copyright@2016 by sapperjiang");
            
            lblText.Text=strMsg;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}