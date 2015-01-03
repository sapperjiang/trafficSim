using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SubSys_MathUtility;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_DataVisualization
{
    public class TimeSpaceDataProvider : ChartDataProvider
    {
        public TimeSpaceDataProvider()
        {
            this.strXAiexTitle = "时间(s)";
            this.strYAiexTitle = "距离(m)";
            this.strHeaderTitle = "时空图";

        }
        internal override MyPoint GetDataPoint(CarInfo ciq)
        {
            return new MyPoint(ciq.iTimeStep, ciq.iPos * this.iCellMeters);
        }

    }
}
