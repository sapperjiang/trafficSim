using System;
using SubSys_SimDriving;

namespace SubSys_SimDriving.TrafficModel
{
	internal class RoadLaneChain :ChainBaseClass<RoadLane>
	{
        internal RoadEdge _ContainerRoadEdge;

        internal new void Add(RoadLane rl)
        {
            if (rl ==null)
            {
                throw new ArgumentNullException();
            }
            base.Add(rl);

            //base.listChain.Sort(new RoadLane());//或者如下
            base.listChain.Sort(new Comparison<RoadLane>(RoadLane.CompareTo));
        }
        internal new void Remove(RoadLane rl)
        {
            if(rl == null)
            {
                throw new ArgumentNullException();
            }
            base.Remove(rl);
            //base.listChain.Sort(new RoadLane());//一个有序的list删除之后仍然有序

        }
    }
	 
}
 
