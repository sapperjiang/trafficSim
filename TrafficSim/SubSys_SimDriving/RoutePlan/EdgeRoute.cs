using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.MathSupport;

namespace SubSys_SimDriving.RoutePlan
{
	internal class EdgeRoute : Route<RoadEdge>
	{
        /// <summary>
        /// 返回下一步要前进的方向,-1表示左转 0表示直行 1表示右转
        /// </summary>
        /// <param name="re">当前车辆所在的道路</param>
        /// <returns></returns>
        internal int getTurnDirection(RoadEdge re)
        {
            RoadEdge reNext  = base.FindNext(re);
            if (reNext == null)//如果到达路径的终点就直行
	        {
		        return 0;
	        }
            return VectorTools.getVectorPos(re.ToVector(),reNext.to.Postion);            
        }
	}
	 
}
 
