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
        /// ʹ����ɢѡ��ģ���е�logitģ�ͽ���·����ѡ��
        /// ����1����ѡ��·��A������2����ѡ��·��B
        /// </summary>
        /// <returns></returns>
        internal override int Select(EdgeRoute routeA,EdgeRoute routeB)
        {
            //��һ��·����·��Ч�ú�
            double iUtilityA = this.GetRouteUtility(routeA);
            //�ڶ���·����·��Ч�ú�
            double iUtilityB = this.GetRouteUtility(routeB);

            //logit�еķ�ĸ
            double dDevider = Math.Exp(iUtilityA)+Math.Exp(iUtilityB);

            //logit ģ��
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
 
