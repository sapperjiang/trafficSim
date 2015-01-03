using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;


namespace GISTranSim.Input
{
    public partial class DataOutputer : Form
    {
        public DataOutputer()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        { 
            LoadData();
            this.BT_SaveOD.Enabled = true;
            base.OnLoad(e);
        }
        DataSet ds ;
        string strFileName = "d:\\TOD.xml";
       
        ISimContext ISC;
        private void BT_SaveOD_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                strFileName = sfd.FileName;
            }
            ds.WriteXml(strFileName);
        }

        private void LoadData()
        {
            ds = new DataSet();
            ISC = SimContext.GetInstance();
            DataTable dt = new DataTable("Data");
            ds.Tables.Add(dt);

            DataColumnCollection dc = dt.Columns;
            try
            {
                dc.Add(new DataColumn("EntityID"));
                dc.Add(new DataColumn("iCarNum"));

                dc.Add(new DataColumn("iTimeStep"));
                dc.Add(new DataColumn("iPos"));
                dc.Add(new DataColumn("iSpeed"));

                int iMeters = SimSettings.iCellWidth;

                foreach (KeyValuePair<int, CarInfoDic> itemEntity in ISC.DataRecorder)
                {
                    foreach (KeyValuePair<int, CarInfoQueue> item in itemEntity.Value)//carinfo Queue
                    {
                        //同一辆车在不同的位置
                        foreach (var itemCarInfo in item.Value)//车辆信息
                        {
                            DataRow dr = dt.NewRow();
                            dr["EntityID"] = itemEntity.Key;
                            dr["iCarNum"] = itemCarInfo.iCarNum;
                            dr["iTimeStep"] = itemCarInfo.iTimeStep;
                            dr["iPos"] = itemCarInfo.iPos * iMeters;
                            dr["iSpeed"] = itemCarInfo.iSpeed * iMeters;
                            dt.Rows.Add(dr);
                        }
                    }
                }
                this.DGV_ODData.DataSource = dt;
                DGV_ODData.Show();
            }
            catch (Exception eX)
            {
                MessageBox.Show("保存失败！" + eX.Message);
            }
        }

        private void BT_Concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
