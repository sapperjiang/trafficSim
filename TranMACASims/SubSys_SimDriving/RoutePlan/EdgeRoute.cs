using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;

namespace SubSys_SimDriving.RoutePlan
{
	public class EdgeRoute : Route<Way>,IEnumerable<Way>
	{
        /// <summary>
        /// 返回下一步要前进的方向,-1表示左转 0表示直行 1表示右转，2表示掉头
        /// </summary>
        /// <param name="re">当前车辆所在的道路</param>
        /// <returns></returns>
        internal int GetDirection(Way re)
        {
            Way reNext  = base.FindNext(re);
            if (reNext == null)//如果到达路径的终点就直行
	        {
		        return 0;
	        }
            return VectorTools.GetVectorPos(re.ToVector(),reNext.ToVector());
            
        }
 
		public override void Add(Way nextWay)
		{
			if (this.routeList.Count>0) {
				if (this.FindPrev(nextWay)==nextWay) {
					ThrowHelper.ThrowArgumentException("相邻两条路线是同一条道路，应该为不同道路");
				}
			}
			
			base.Add(nextWay);
		}
        public IEnumerator<Way> GetEnumerator()
        {
            return routeList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
      
        
    }
	 
}
 
