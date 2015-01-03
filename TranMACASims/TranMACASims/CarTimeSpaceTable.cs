using SubSys_SimDriving;
//using SubSys_DataMgr;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimDrivingContext;

namespace SubSys_SimDriving
{
	/**
	 * 车辆时空表，在仿真需要数据输出的时候，
	 * 利用它保存一辆车的时空信息，
	 * 可以作为一个接口提供
	 * 
	 */
	public abstract class CarTimeSpaceTable
	{
		public int iTimeStep;
		 
        //public Point CarPosition;
		 
		private int iRoadSegID;
		 
        //private CarTimeSpaceTableAnalyzer carTimeSpaceTableAnalyzer;
		 
        //private CarTimeSpaceTableAnalyzer carTimeSpaceTableAnalyzer;
		 
		private Route route;
		 
		private CarModel carModel;
		 
		private CarModelHashTable carModelHashTable;
		 
		private RunningStatusRoadSegHashTable runningStatusRoadSegHashTable;
		 
	}
	 
}
 
