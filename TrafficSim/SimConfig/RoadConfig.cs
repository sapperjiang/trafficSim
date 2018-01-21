﻿using System;

using System.Linq;

using System.Windows.Forms;


namespace TrafficSim
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
