﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SubSys_MathUtility;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using System.Windows.Forms.DataVisualization.Charting;

namespace SubSys_DataVisualization
{
   public class TimeSpeedCharter: DataCharter
    {
       public TimeSpeedCharter()
        { 
            this.strXAiexTitle = "时间(s)";
            this.strYAiexTitle = "速度(m/s)";
            this.strHeaderTitle = "速度时间图";

        }
        internal override OxyzPointF CalcChartData(CarInfo ci)
        {
            return new OxyzPointF(ci.iTimeStep, ci.iSpeed * this.iCellMeters);
        }

        public override void FillSerisCollection(SeriesCollection dataSRC)
        {
            int iC = 0;
            foreach (IDataRecorder<int, CarTrack> itemEntity in ISC.DataRecorder.Values)
            {
                foreach (KeyValuePair<int, CarTrack> item in itemEntity)//carinfo Queue
                {
                    if (iC++==2)
                    {
                        break;
                    }
                    Series dataI = dataSRC.FindByName(item.Key.ToString());

                    //同一辆车在不同的位置
                    if (dataI == null)
                    {
                        dataI = new Series(item.Key.ToString());
                        dataI.MarkerStyle = MarkerStyle.Diamond;
                        dataI.ChartType = SeriesChartType.Line;
                        dataSRC.Add(dataI);
                    }

                    foreach (var itemCarInfo in item.Value)//车辆信息
                    {
                        OxyzPointF p = this.CalcChartData(itemCarInfo);
                        dataI.Points.AddXY((double)p._X, (double)p._Y);
                    }
                }
                break;
            }
        }

    }
}
