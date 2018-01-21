using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.SysSimContext
{
	/**
	 * 利用这个哈希表查询到要查询的车辆的Id
	 * 然后找到车辆的RouteID，
	 * 然后查询RoadSegHastTable找到每辆车
	 * 在每个路段的信息
	 */
    public   class CarModelHTable : StaticSysTable<int, Car>
	{
        //private Route route;
	}
	 
}
 
