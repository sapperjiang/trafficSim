﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_DataVisualization
{
     public abstract class DataCharter
    {
        public string strXAiexTitle;// = "距离(m)";
        public string strYAiexTitle;// = "时间(s);";
        public string strHeaderTitle;// = "时空图时间(s);";
        protected ISimContext ISC = SimContext.GetInstance();
        public int iCellMeters = SimSettings.iCellWidth;

        public virtual void FillSerisCollection(SeriesCollection dataSRC)
        {
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

                    foreach (var carInfo in item.Value)//车辆信息
                    {
                         OxyzPointF p = this.CalcChartData(carInfo);
                         dataI.Points.AddXY((double)p._X, (double)p._Y);
                    }
                }
            }
        }
        internal abstract OxyzPointF CalcChartData(CarInfo ciq);
      
    }
}
