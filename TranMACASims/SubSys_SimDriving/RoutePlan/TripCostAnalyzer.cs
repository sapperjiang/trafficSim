using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.RoutePlan
{
	internal abstract class TripCostAnalyzer
	{
		internal virtual double GetTripCost(RoadEdge re)
		{
			return 12;
		}
		 
	}
	 
}
 
