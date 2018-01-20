using System;
using System.Collections.Generic;
using SubSys_SimDriving;

namespace SubSys_SimDriving.TrafficModel
{
	public class Lanes :List<Lane>
	{
        internal Way _containerWay;

        internal new void Add(Lane rl)
        {
            if (rl ==null)
            {
                throw new ArgumentNullException();
            }
            base.Add(rl);
            base.Sort(new Comparison<Lane>(Lane.CompareTo));
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
    public class LaneDics : StaticDics<int, Lane>
    {
    }

}

