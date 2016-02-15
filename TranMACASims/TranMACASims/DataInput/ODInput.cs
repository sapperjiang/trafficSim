using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;


namespace GISTranSim.Data
{
    public partial class ODInOutMgr : Form
    {
        public ODInOutMgr()
        {
            InitializeComponent();
        }
        DataSet ds ;
        string strFileName = "d:\\TOD.xml";//
        ISimContext ISC;
        private void BT_SaveOD_Click(object sender, EventArgs e)
        {
            ds = new DataSet();
            ds.ReadXml(strFileName);
            this.DGV_ODData.DataSource = ds.Tables[1];
            DGV_ODData.Show();

            ISC = SimContext.GetInstance();
            DataTable dt = ds.Tables[1];
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

                ds.WriteXml(strFileName);
                MessageBox.Show("保存成功");
            }
            catch (Exception eX)
            {
                MessageBox.Show("保存失败！"+eX.Message); 
            }
        }

        private void BT_Concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
