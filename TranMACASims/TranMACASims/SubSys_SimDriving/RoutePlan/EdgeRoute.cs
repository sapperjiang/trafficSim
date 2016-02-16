using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.MathSupport;

namespace SubSys_SimDriving.RoutePlan
{
	internal class EdgeRoute : Route<RoadEdge>
	{
        /// <summary>
        /// ������һ��Ҫǰ���ķ���,-1��ʾ��ת 0��ʾֱ�� 1��ʾ��ת
        /// </summary>
        /// <param name="re">��ǰ�������ڵĵ�·</param>
        /// <returns></returns>
        internal int getTurnDirection(RoadEdge re)
        {
            RoadEdge reNext  = base.FindNext(re);
            if (reNext == null)//�������·�����յ��ֱ��
	        {
		        return 0;
	        }
            return VectorTools.getVectorPos(re.ToVector(),reNext.to.Postion);            
        }
	}
	 
}
 
