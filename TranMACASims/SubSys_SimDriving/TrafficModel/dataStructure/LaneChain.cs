using System;
using SubSys_SimDriving;

namespace SubSys_SimDriving.TrafficModel
{
	public class LaneChain :AbstractChain<Lane>
	{
        internal Way _ContainerRoadEdge;

        internal new void Add(Lane rl)
        {
            if (rl ==null)
            {
                throw new ArgumentNullException();
            }
            base.Add(rl);
            base.listChain.Sort(new Comparison<Lane>(Lane.CompareTo));
        }
        internal new void Remove(Lane rl)
        {
            if(rl == null)
            {
                throw new ArgumentNullException();
            }
            base.Remove(rl);

        }
    }
	 
}
 
