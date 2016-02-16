using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.RoutePlan
{
	internal abstract class TripCostAnalyzer
	{
		private CarModel carModel;
		 
		internal virtual int GetTripCost(RoadEdge re)
		{
			return 12;
		}
		 
	}
	 
}
 
