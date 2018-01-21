using System;
using SubSys_SimDriving;

namespace SubSys_SimDriving.TrafficModel
{
	public class RoadLaneChain :ChainBaseClass<RoadLane>
	{
        public RoadEdge _ContainerRoadEdge;

        public new void Add(RoadLane rl)
        {
            if (rl ==null)
            {
                throw new ArgumentNullException();
            }
            base.Add(rl);

            //base.listChain.Sort(new RoadLane());//��������
            base.listChain.Sort(new Comparison<RoadLane>(RoadLane.CompareTo));
        }
        public new void Remove(RoadLane rl)
        {
            if(rl == null)
            {
                throw new ArgumentNullException();
            }
            base.Remove(rl);
            //base.listChain.Sort(new RoadLane());//һ�������listɾ��֮����Ȼ����

        }
    }
	 
}
 
