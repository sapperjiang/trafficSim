using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    [System.Obsolete("���õ���Ʋ�ʹ����")]
	internal abstract class CellDevolveMediator
	{
        /// <summary>
        /// Transfer �����н��߸øɵ�����
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="re"></param>
        internal abstract void Transfer(Cell ca,RoadEntity re);
        
        ///// <summary>
        ///// Accept�Ǿ���Ĳ����߸ɵ�����
        ///// </summary>
        ///// <param name="ca"></param>
        //internal abstract void Accept(Cell ca);

	}
    internal class CellDevolver:CellDevolveMediator
    {
        /// <summary>
        /// �������Բ���Ҫ�Լ����Ǹ���ֻҪ���͵�RoadNode����RoadEdge�Ϳ�����
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
 
