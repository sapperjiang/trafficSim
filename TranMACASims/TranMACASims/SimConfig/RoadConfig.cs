using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SubSys_DataManage;
using SubSys_Graphics;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.Service;
using SubSys_SimDriving.TrafficModel;


namespace GISTranSim
{
	public partial class RoadConfig : Form
	{
		public RoadConfig()
		{
			InitializeComponent();
		}

		internal string strRoadName;
		internal int iRoadLength = 0;
		//        internal
		//        internal int iSimSpeed = 0;
		internal int iStraghtCount =1;
		internal int iRightCount =1;
		internal int iLeftCount =1;
		//        internal int iSimSpeed = 0;
		//        internal double dRatio=0.85;

		private void BT_ConFirm_Click(object sender, EventArgs e)
		{
			
			try {
				
				this.iRoadLength = Convert.ToInt32(this.TB_RoadLength.Text);
				
				this.iLeftCount = Convert.ToInt32(this.TB_LeftLaneCount.Text);
				this.iStraghtCount = Convert.ToInt32(this.TB_StraightLaneCount.Text);
				this.iRightCount = Convert.ToInt32(this.TB_RightLaneCount.Text);
				
			//	Way  roadEdge = WayFactory.BuildOneWay(new Point(),new Point(),this.iLeftCount,this.iStraghtCount,this.iRightCount);//);
				
				roadEdge .Name  =this.TB_RoadName.Text;
				
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
