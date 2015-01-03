using System.Collections.Generic;

using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.SysSimContext
{
    internal class RunningStatusRoadSegHashTable : StaticSysTable<int, Dictionary<int, CellChain>>
	{
		private int iTimeStep;
		 
		private CarTimeSpaceTable carTimeSpaceTable;
		 
	}
	 
}
 
