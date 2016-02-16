using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    [System.Obsolete("不好的设计不使用了")]
	internal abstract class CellDevolveMediator
	{
        /// <summary>
        /// Transfer 才是中介者该干的事情
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="re"></param>
        internal abstract void Transfer(Cell ca,RoadEntity re);
        
        ///// <summary>
        ///// Accept是具体的参与者干的事情
        ///// </summary>
        ///// <param name="ca"></param>
        //internal abstract void Accept(Cell ca);

	}
    internal class CellDevolver:CellDevolveMediator
    {
        /// <summary>
        /// 甚至可以不需要自己找那个人只要发送到RoadNode或者RoadEdge就可以了
        /// </summary>
        /// <param name="ce"></param>
        /// <param name="re"></param>
        internal override void Transfer(Cell ce, RoadEntity re)
        {
            if (re is RoadNode)
            {
                //ce.cmCarModel.EdgeRoute;
            }
            if (true)
            {
                
            }
            //throw new System.NotImplementedException();
        }
    }
	 
}
 
