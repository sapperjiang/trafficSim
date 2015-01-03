using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;

namespace SubSys_SimDriving.RoutePlan
{
	public class EdgeRoute : Route<RoadEdge>,IEnumerable<RoadEdge>
	{
        /// <summary>
        /// 返回下一步要前进的方向,-1表示左转 0表示直行 1表示右转，2表示掉头
        /// </summary>
        /// <param name="re">当前车辆所在的道路</param>
        /// <returns></returns>
        internal int GetSwerve(RoadEdge re)
        {
            RoadEdge reNext  = base.FindNext(re);
            if (reNext == null)//如果到达路径的终点就直行
	        {
		        return 0;
	        }
            return VectorTools.GetVectorPos(re.ToVector(),reNext.ToVector());            
        }

        public IEnumerator<RoadEdge> GetEnumerator()
        {
            return routeList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
	 
}
 
