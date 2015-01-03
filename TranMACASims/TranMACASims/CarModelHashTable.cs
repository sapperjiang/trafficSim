using SubSys_SimDriving.SimDrivingStaticTable;
using SubSys_SimDriving;

namespace SubSys_SimDriving.SimDrivingStaticTable
{
	/**
	 * 利用这个哈希表查询到要查询的车辆的Id
	 * ，然后找到车辆的RouteID，
	 * 然后查询RoadSegHastTable找到每辆车
	 * 在每个路段的信息
	 */
	public class CarModelHashTable : StaticSysTable
	{
		private Route route;
		 
		private Route[] route;
		 
		private CarTimeSpaceTable carTimeSpaceTable;
		 
	}
	 
}
 
