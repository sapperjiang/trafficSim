using System;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;

namespace SubSys_SimDriving.RoutePlan
{
    internal abstract class ChoiceMaker 
	{
        internal virtual int Select(EdgeRoute routeA, EdgeRoute routeB)
		{
            return 0;
		}
		 
	}

    internal class RouteChoiceMaker : ChoiceMaker
    {
        TripCostAnalyzer tca = new JamTripCostAnalyzer();

        private double GetRouteUtility(EdgeRoute er)
        {
            double iUtilitySum = 0;
            foreach (var item in er)
            {
                iUtilitySum +=  tca.GetTripCost(item); 
            }
            return iUtilitySum;
        }

        /// <summary>
        /// 使用离散选择模型中的logit模型进行路径的选择。
        /// 返回1代表选择路径A，返回2代表选择路径B
        /// </summary>
        /// <returns></returns>
        internal override int Select(EdgeRoute routeA,EdgeRoute routeB)
        {
            //第一个路径的路段效用和
            double iUtilityA = this.GetRouteUtility(routeA);
            //第二个路径的路段效用和
            double iUtilityB = this.GetRouteUtility(routeB);

            //logit中的分母
            double dDevider = Math.Exp(iUtilityA)+Math.Exp(iUtilityB);

            //logit 模型
            double dProbA = 1 - Math.Exp(iUtilityB) / dDevider;

            Random rd = new Random(0);
            if (dProbA >= rd.NextDouble())
	        {
                return 1;
	        } 
            return 2;

        }

    }
	 
}
 
