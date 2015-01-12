using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_DataVisualization
{
   public class MeanSpeedDataProvider: ChartDataProvider
    {
       public MeanSpeedDataProvider()
        { 
            this.strXAiexTitle = "时间(s);";
            this.strYAiexTitle = "平均速度(m/s)";
            this.strHeaderTitle = "平均速度时间图";

        }
       public override void FillSerisCollection(System.Windows.Forms.DataVisualization.Charting.SeriesCollection dataSRC)
       {
           Series srMeanSpeed = new Series();
           srMeanSpeed.ChartType = SeriesChartType.Line;
           srMeanSpeed.MarkerStyle = MarkerStyle.Diamond;

           foreach (IDataRecorder<int, CarInfoQueue> itemEntity in ISC.DataRecorder.Values)
           {
               int iRecordCount = 0;
               foreach (KeyValuePair<int, CarInfoQueue> item in itemEntity)//carinfo Queue
               { 
                   iRecordCount = item.Value.Count;//车辆的记录
                   break;
               }

               int iSpeedSum = 0;
               int iCarCount =0;
               for (int i = 0; i < iRecordCount; i++)
			    {
                   int iTimeStep = 0;
			         foreach (var key in itemEntity.Keys)//carinfo Queue
                    { 
                         CarInfo ci = itemEntity[key][i];
                         if (ci!=null)
	                    {
                             iTimeStep = ci.iTimeStep;
		                    iSpeedSum+=ci.iSpeed;
                            iCarCount++;
	                    }else{
                        continue;
                        }
                     }
                     srMeanSpeed.Points.AddXY(iTimeStep, this.iCellMeters * iSpeedSum / iCarCount);
                       
	            }
               }

           dataSRC.Add(srMeanSpeed);

           }
       
        internal override OxyzPointF GetDataPoint(CarInfo ciq)
        {
            return new OxyzPointF(ciq.iTimeStep, ciq.iPos * this.iCellMeters);
        }

    }
}
