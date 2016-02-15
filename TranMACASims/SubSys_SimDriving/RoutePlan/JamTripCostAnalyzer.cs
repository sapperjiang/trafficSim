using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;

namespace SubSys_SimDriving.RoutePlan
{
    internal class JamTripCostAnalyzer:TripCostAnalyzer
	{
		internal override double GetTripCost(Way re)
		{
//            int iCapacity  = 0;
//            int iVolumn = 0;
//            SignalLight sl=null;
//            foreach (var item in re.Lanes)
//	        {
//                sl = item.SignalLight;
//		         iCapacity+= item.CellCount;
//                        iVolumn +=item.iLength;
//	        }
//            int iSignalTime = 0;
//            if (sl!=null)
//            {
//                iSignalTime= sl.RedLength; 
//            }
//			return (re.iLength/3+iSignalTime)*(1+2.62*iCapacity/iVolumn);
return 0.0f;
		}
		 
	}
	 
}
 
