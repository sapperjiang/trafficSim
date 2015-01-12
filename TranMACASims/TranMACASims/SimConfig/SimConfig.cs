using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using SubSys_DataManage;
using SubSys_Graphics;
using SubSys_SimDriving.Agents;

using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.SysSimContext.Service;
using SubSys_SimDriving.TrafficModel;

namespace GISTranSim
{
	public partial class SimConfig : Form
	{
		public SimConfig()
		{
			InitializeComponent();
		}

		internal int iCarCount = 0;
		internal int iRoadLength = 0;
		internal int iSimSpeed = 0;
		internal double dRatio=0.85;

		private void BT_ConFirm_Click(object sender, EventArgs e)
		{
			try {
				
				this.iCarCount = Convert.ToInt32(this.TB_CarCount.Text);
				this.iRoadLength = Convert.ToInt32(this.TB_RoadLength.Text);
				this.iSimSpeed = Convert.ToInt32(this.TB_SimInterval.Text);
				this.dRatio = Convert.ToDouble(this.TB_Ratio.Text);
				this.DialogResult = DialogResult.OK;
				
			} catch (Exception) {
				
				MessageBox.Show("参数错误！");
			}
		}
		/// <summary>
		/// 点击取消按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void BT_ConcelClick(object sender, System.EventArgs e)
		{
			this.Close();
			//throw new NotImplementedException();
		}
	}
}
