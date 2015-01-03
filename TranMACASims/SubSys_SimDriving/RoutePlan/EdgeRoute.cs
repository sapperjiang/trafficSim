using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;

using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;

namespace SubSys_SimDriving.RoutePlan
{
	public class EdgeRoute : Route<RoadEdge>,IEnumerable<RoadEdge>
	{
        /// <summary>
        /// ������һ��Ҫǰ���ķ���,-1��ʾ��ת 0��ʾֱ�� 1��ʾ��ת��2��ʾ��ͷ
        /// </summary>
        /// <param name="re">��ǰ�������ڵĵ�·</param>
        /// <returns></returns>
        internal int GetSwerve(RoadEdge re)
        {
            RoadEdge reNext  = base.FindNext(re);
            if (reNext == null)//�������·�����յ��ֱ��
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
 
