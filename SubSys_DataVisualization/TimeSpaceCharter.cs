using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SubSys_MathUtility;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_DataVisualization
{
    public class TimeSpaceCharter : DataCharter
    {
        public TimeSpaceCharter()
        {
            this.strXAiexTitle = "时间(s)";
            this.strYAiexTitle = "距离(m)";
            this.strHeaderTitle = "时空图";

        }
        internal override OxyzPointF CalcChartData(CarInfo ciq)
        {
            return new OxyzPointF(ciq.iTimeStep, ciq.iDrivedMileage*this.iCellMeters);
        }

    }
}
