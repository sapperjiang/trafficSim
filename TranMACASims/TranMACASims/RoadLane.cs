using System;
using SubSys_SimDriving;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// ��ͨʵ������ĳ���
    /// </summary>
    public class RoadLane : RoadEntity, IComparable<RoadLane>, IComparer<RoadLane>
	{
        public RoadEdge parentEntity;

        public RoadLane(RoadEdge re)
        {
            parentEntity = re;
            this.laneType = LaneType.Straight;//Ĭ��Ϊֱ�г���
        }
        public RoadLane(RoadEdge re,LaneType lt)
        {
            parentEntity = re;
            this.laneType = lt;
        }
		public LaneType laneType;
		 
		public CACellChain cellChain = new CACellChain();

        public override void UpdateStatus()
        {
            //�����첽��Ϣ
            for (int i = 0; i < this.asynRuleChain.Count; i++)
            {
                UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
                visitorRule.VisitUpdate(this);//.VisitUpdate();
            }
            ////����ͬ����Ϣ
            //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
            //    }
            //}
        }
		 
        public int CompareTo(RoadLane other)
        {
            if (this.laneType == other.laneType)
            {
                return 0;
            }
            return this.laneType > other.laneType ? 1 : -1;
        }
        public int Compare(RoadLane x, RoadLane y)
        {
            return x.CompareTo(y);
        }
        /// <summary>
        /// ��̬����
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int CompareTo(RoadLane from, RoadLane to)
        {
            return from.CompareTo(to);
        }
    }
	 
}
 
